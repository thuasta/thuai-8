using System.Text.Json;
using System.Text.Json.Serialization;

using System.Collections.Concurrent;

namespace Thuai.Server.Recorder;

public class RecordPage
{
    /// <summary>
    /// Number of records.
    /// </summary>
    [JsonIgnore]
    public int Length => Records.Count;

    [JsonPropertyName("type")]
    public string Type => "record";

    [JsonPropertyName("records")]
    public ConcurrentQueue<RecordElement> Records { get; private set; } = new();

    [JsonIgnore]
    private JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Add a record.
    /// </summary>
    /// <param name="record">Record to save.</param>
    public void Record(params Protocol.IRecordable[] records)
    {
        Records.Enqueue(new RecordElement { Record = [.. records] });
    }

    /// <summary>
    /// Export the records and clear the records.
    /// </summary>
    /// <returns>Serialized record.</returns>
    public string Export()
    {
        string result = JsonSerializer.Serialize((object)this, _jsonSerializerOptions);
        Records.Clear();
        return result;
    }
}
