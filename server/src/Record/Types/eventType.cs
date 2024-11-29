using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Thuai.GameServer.Recorder;

public record eventType
{
    [JsonPropertyName("eventType")]
    public string? eventTypeOfABCD { get; set; }
}
