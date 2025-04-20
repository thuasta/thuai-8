using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class JsonLoader
{
    public static JObject LoadRecord(string path)
    {
        if (!File.Exists(path)) return CreateError("File not found");

        var records = new List<RecordMetadata>();
        var extension = Path.GetExtension(path).ToLower();

        try
        {
            if (extension == ".zip" || extension == ".dat")
            {
                ProcessZip(path, records);
            }
            else if (extension == ".json")
            {
                ProcessJson(path, records);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Processing failed: {ex.Message}");
            return CreateError("Processing failed");
        }

        if (records.Count == 0) return CreateError("No valid records");

        var mergedRecords = MergeRecords(records
            .OrderBy(r => r.TotalTicks)
            .Select(r => r.Data));

        return new JObject
        {
            ["type"] = "record",
            ["records"] = mergedRecords
        };
    }

    private static void ProcessZip(string path, List<RecordMetadata> output)
    {
        using var zip = ZipFile.OpenRead(path);
        var entries = zip.Entries
            .Where(e => !IsDirectory(e) && !e.FullName.Contains("level"))
            .ToArray();

        Parallel.ForEach(entries, entry =>
        {
            try
            {
                using var stream = entry.Open();
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();
                var totalTicks = ExtractTotalTicks(JObject.Parse(json));

                if (totalTicks != -1)
                {
                    var data = JObject.Parse(json);
                    lock (output)
                    {
                        output.Add(new RecordMetadata(totalTicks, data));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to process {entry.FullName}: {ex.Message}");
            }
        });
    }

    private static int ExtractTotalTicks(JObject record)
    {
        foreach (JObject r in record["records"].Cast<JObject>())
        {
            foreach (JObject entry in r["record"].Cast<JObject>())
            {
                if (entry["messageType"]?.ToString() == "STAGE_INFO")
                {
                    return entry["totalTicks"]?.Value<int>() ?? -1;
                }
            }
        }
        return -1;
    }

    private static JArray MergeRecords(IEnumerable<JObject> records)
    {
        var merged = new JArray();
        foreach (var record in records)
        {
            if (record["records"] is JArray recordsArray)
            {
                merged.Merge(recordsArray, new JsonMergeSettings
                {
                    MergeArrayHandling = MergeArrayHandling.Concat
                });
            }
        }
        return merged;
    }

    private static void ProcessJson(string path, List<RecordMetadata> output)
    {
        var json = File.ReadAllText(path);
        var obj = JObject.Parse(json);
        var totalTicks = ExtractTotalTicks(obj);
        if (totalTicks != -1)
        {
            output.Add(new RecordMetadata(totalTicks, obj));
        }
    }

    private static bool IsDirectory(ZipArchiveEntry entry) =>
        entry.FullName.EndsWith("/");

    private static JObject CreateError(string message)
    {
        Debug.LogError(message);
        return new JObject { ["error"] = message };
    }

    private struct RecordMetadata
    {
        public int TotalTicks { get; }
        public JObject Data { get; }

        public RecordMetadata(int totalTicks, JObject data)
        {
            TotalTicks = totalTicks;
            Data = data;
        }
    }
}