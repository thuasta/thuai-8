using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Thuai.GameServer.Recorder;

public record BattleUpdate : IRecord
{
    [JsonPropertyName("messageType")]
    public string messageType => "BATTLE_UPDATE";

    [JsonPropertyName("currentTicks")]
    public int currentTicks { get; set; }

    [JsonIgnore]
    public JsonNode Json => JsonNode.Parse(JsonSerializer.Serialize(this))!;
    [JsonPropertyName("players")]

    public List<playerType> Players { get; set; } = new List<playerType>();
    [JsonPropertyName("events")]
    public List<eventType> Events { get; set; } = new List<eventType>();
}