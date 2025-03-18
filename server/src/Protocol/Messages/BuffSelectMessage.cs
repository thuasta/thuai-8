using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record BuffSelectMessage : Message, IRecordable
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BUFF_SELECT";

    [JsonPropertyName("details")]
    public required List<Detail> Details { get; init; }
}

public record Detail
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("buff")]
    public required string Buff { get; init; }
}
