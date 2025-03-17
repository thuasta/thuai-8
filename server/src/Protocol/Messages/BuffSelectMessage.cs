using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record BuffSelectMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BUFF_SELECT";

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("buff")]
    public required string Buff { get; init; }
}
