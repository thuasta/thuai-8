using System.Text.Json.Serialization;

namespace Thuai.Server.Recorder;

public record RecordElement
{
    [JsonPropertyName("record")]
    public List<Protocol.IRecordable> Record { get; init; } = [];
}
