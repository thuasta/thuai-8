using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Thuai.GameServer.Recorder;

public record BuffSelect : IRecord
{
    [JsonPropertyName("messageType")]
    public string messageType => "BUFF_SELECT";

    [JsonPropertyName("currentTicks")]
    public int currentTicks { get; set; }

    [JsonIgnore]
    public JsonNode Json => JsonNode.Parse(JsonSerializer.Serialize(this))!;
    [JsonPropertyName("buff")]
    public string buff { get; set; } = "";
}