using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record BuffSelectMessage : Message, IRecordable
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BUFF_SELECT";

    [JsonPropertyName("chosenBuffs")]
    public required List<ChosenBuff> ChosenBuffs { get; init; }

    [JsonPropertyName("availableBuffs")]
    public required List<string> AvailableBuffs { get; init; }
}

public record ChosenBuff
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("buff")]
    public required string Buff { get; init; }
}
