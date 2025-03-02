using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record AvailableBuffsMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "AVAILABLE_BUFFS";

    [JsonPropertyName("AvailableBuffs")]
    public List<string> AvailableBuffs { get; init; } = [];

}