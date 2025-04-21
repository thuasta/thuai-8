using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class JsonUtility
{
    public static JObject LoadRecord(string path, IProgress<float> progress = null)
    {
        if (!File.Exists(path)) return CreateErrorResult("File not found");

        var records = new List<JObject>();
        var extension = Path.GetExtension(path).ToLower();

        try
        {
            if (extension == ".zip" || extension == ".dat")
            {
                records.AddRange(ProcessZipArchive(path, progress));
            }
            else if (extension == ".json")
            {
                if (ProcessJsonFile(path) is JObject jsonObj)
                    records.Add(jsonObj);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Processing failed: {ex.Message}");
            return CreateErrorResult("Processing failed");
        }

        // 优化排序阶段
        var validRecords = records
            .Select((r, i) => (Index: i, FirstTick: GetFirstTickOptimized(r)))
            .Where(x => x.FirstTick != -1)
            .ToList();

        if (validRecords.Count == 0) return CreateErrorResult("No valid records found");

        // 并行排序（当记录量很大时有效）
        validRecords.Sort((a, b) => a.FirstTick.CompareTo(b.FirstTick));

        // 优化合并阶段
        var mergedRecords = new List<JToken>();
        foreach (var item in validRecords)
        {
            if (records[item.Index]["records"] is JArray recordsArray)
                mergedRecords.AddRange(recordsArray.Children());
        }

        return new JObject
        {
            ["type"] = "record",
            ["records"] = new JArray(mergedRecords)
        };
    }

    private static List<JObject> ProcessZipArchive(string path, IProgress<float> progress = null)
    {
        using var zip = ZipFile.OpenRead(path);
        var entries = zip.Entries.Where(e =>
            !IsDirectory(e) &&
            !e.FullName.Contains("level") &&
            e.Length > 0
        ).ToList();

        var result = new List<JObject>(entries.Count);

        // 调整并行度为物理核心数
        Parallel.ForEach(entries, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (entry, state, index) =>
        {
            try
            {
                using var stream = entry.Open();
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();
                if (JObject.Parse(json) is JObject obj)
                    lock (result) { result.Add(obj); }

                // 更新进度
                float progressValue = (float)(index + 1) / entries.Count;
                progress?.Report(progressValue);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to process {entry.FullName}: {ex.Message}");
            }
        });

        return result;
    }

    private static JObject ProcessJsonFile(string path)
    {
        try
        {
            return JObject.Parse(File.ReadAllText(path));
        }
        catch (Exception ex)
        {
            Debug.LogError($"JSON parse failed: {ex.Message}");
            return null;
        }
    }

    // 优化后的FirstTick获取方法
    private static int GetFirstTickOptimized(JObject record)
    {
        if (record["records"] is not JArray recordsArray) return -1;

        foreach (JObject entry in recordsArray.Children<JObject>())
        {
            if (entry["record"] is not JArray recordArray) continue;

            foreach (JObject message in recordArray.Children<JObject>())
            {
                if (message["messageType"]?.ToString() == "STAGE_INFO")
                {
                    return message["totalTicks"]?.Value<int>() ?? -1;
                }
            }
        }
        return -1;
    }

    private static bool IsDirectory(ZipArchiveEntry entry) =>
        entry.FullName.EndsWith("/");

    private static JObject CreateErrorResult(string message)
    {
        Debug.LogError(message);
        return new JObject { ["error"] = message };
    }
}