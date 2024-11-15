using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Thuai.GameServer.Recorder;

public record Error : IRecord
{
    [JsonPropertyName("errorCode")]
    public int errorCode { get; set; }
    [JsonPropertyName("message")]
    public string message { get; set; } = "";
    [JsonIgnore]
    public JsonNode Json => JsonNode.Parse(JsonSerializer.Serialize(this))!;
}
