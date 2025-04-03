using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record AvailableBuffsMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "AVAILABLE_BUFFS";

    [JsonPropertyName("buffs")]
    public List<string> AvailableBuffs { get; init; } = [];

}