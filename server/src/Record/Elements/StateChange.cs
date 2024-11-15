using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Thuai.GameServer.Recorder;

public record StageChange : IRecord 
{
    [JsonPropertyName("messageType")]
    public string messageType => "STAGE_CHANGE";
    [JsonPropertyName("targetStage")]
    public string targetStage { get; set; } = "";
    [JsonIgnore]
    public JsonNode Json => JsonNode.Parse(JsonSerializer.Serialize(this))!;
}