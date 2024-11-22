using System.Text.Json.Serialization;

namespace GameServer.Connection;

public record AvailableBuffs : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "AVAILABLE_BUFFS";

    [JsonPropertyName("AvailableBuffs")]
    public List<string> AvailableBuffs { get; init; } = new();

}