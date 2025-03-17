using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record StageInfoMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "STAGE_INFO";

    [JsonPropertyName("stage")]
    public required Scheme.Stage Stage { get; init; }

    [JsonPropertyName("totalTicks")]
    public required int TotalTicks { get; init; }
}
