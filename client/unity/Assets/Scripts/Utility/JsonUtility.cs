using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;

public class JsonUtility
{    
    public static JObject UnzipRecord(string path)
    {
        // Load all the record entry
        List<JObject> allRecordJsonObject = new();

        if (Directory.Exists(path))
        {
            Debug.Log("Record is a Directory.");
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            //Loop through each file
            foreach (string file in files)
            {
                try
                {
                    if (!file.Contains("level.dat") && !file.EndsWith("/") && !file.Contains(".meta"))
                    {
                        using (Stream stream = File.OpenRead(file))
                        {
                            // Unzip the record
                            ZipArchive recordZipArchive = new(stream);
                            StreamReader recordStreamReader = new(recordZipArchive.Entries[0].Open());
                            allRecordJsonObject.Add((JObject)JToken.ReadFrom(new JsonTextReader(recordStreamReader)));
                            Debug.Log(recordStreamReader.ReadToEnd().ToString());
                        }
                    }
                }
                catch
                {

                }
            }
        }
        else if (
            File.Exists(path)
            && (Path.GetExtension(path).Equals(".zip", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dat", StringComparison.OrdinalIgnoreCase))
        )
        {
            Debug.Log("Record is a Zipped File.");
            ZipArchive ncLevelDataZipFile = ZipFile.OpenRead($"{path}");
            foreach (ZipArchiveEntry recordEntry in ncLevelDataZipFile.Entries)
            {
                try
                {
                    Debug.Log($"Unzipped record file name: {recordEntry.FullName}");
                    // If the recordEntry is not folder and not level
                    if (!recordEntry.FullName.Contains("level") && !recordEntry.FullName.EndsWith("/"))
                    {
                        StreamReader recordStreamReader = new(recordEntry.Open());
                        allRecordJsonObject.Add((JObject)JToken.ReadFrom(new JsonTextReader(recordStreamReader)));
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
        else if(File.Exists(path) && (Path.GetExtension(path).Equals(".json", StringComparison.OrdinalIgnoreCase)|| Path.GetExtension(path).Equals(".dat", StringComparison.OrdinalIgnoreCase)))
        {
            try
            {
                string jsonContent = File.ReadAllText(path);
                JObject jsonObj = JObject.Parse(jsonContent);
                allRecordJsonObject.Add(jsonObj);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Fail to open the json record : {ex.Message}");
            }
        }

        if (allRecordJsonObject.Count == 0)
            throw new Exception("Record data not found in zip archive.");

        // Compute the first tick
        // pair<int index, int tick>
        (int, int)[] indexAndTicks = new (int, int)[allRecordJsonObject.Count];
        int nowRecordIndex = 0;


        foreach (JObject jsonObject in allRecordJsonObject)
        {
            indexAndTicks[nowRecordIndex].Item1 = nowRecordIndex;
            // If the record file is wrong, then the record will not be added. So let initial tick equal to -1
            indexAndTicks[nowRecordIndex].Item2 = -1;
            //ZipFile.OpenRead(recordDataEntry.FullName);

            // Find the first tick;
            JArray records = (JArray)jsonObject["records"];
            if (records != null && records.Count > 0)
            {
                foreach (JObject recordInfo in records)
                {
                    JArray record = (JArray)recordInfo["record"];
                    if (record == null || record.Count == 0) continue;

                    foreach (JObject recordItem in record)
                    {
                        JToken messageTypeToken = recordItem["messageType"];
                        if (messageTypeToken?.ToString() == "STAGE_INFO")
                        {
                            JToken tickToken = recordItem["totalTicks"];
                            if (tickToken != null && tickToken.Type == JTokenType.Integer)
                            {
                                indexAndTicks[nowRecordIndex].Item2 = (int)tickToken;
                                break; // 找到第一个有效tick后跳出
                            }
                        }
                    }
                    // 如果已找到有效tick则不再检查后续record
                    if (indexAndTicks[nowRecordIndex].Item2 != -1) break;
                }
                nowRecordIndex++;
            }
        }
        // Rearrange the order of record file according to their first ticks
        List<(int, int)> indexAndTicksList = indexAndTicks.ToList<(int, int)>();
        indexAndTicksList.Sort((x, y) => x.Item2.CompareTo(y.Item2));

        foreach ((int, int) it in indexAndTicksList) {
            Debug.Log($"RecordInfo: {it.Item1},{it.Item2}");
        }

        // Write the json obj according to the order
        JObject recordJsonObject = new()
        {
            {"type","record" },
            { "records", new JArray() }
        };

        foreach ((int, int) indexAndTick in indexAndTicksList)
        {
            if (indexAndTick.Item2 != -2)
            {
                // Serial number in allRecordJsonObject: indexAndTick.Item1
                JObject jsonObject = allRecordJsonObject[indexAndTick.Item1];
                JArray records = (JArray)jsonObject["records"];

                // Append
                ((JArray)recordJsonObject["records"]).Merge(records);
            }
        }
        // Sort the final array according to tick
        JArray allRecordsArray = (JArray)recordJsonObject["records"];

        //allRecordJsonObject.OrderBy(record => (int)record["tick"]);

        return recordJsonObject;
    }    
}
