using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record StageInfoMessage : Message, IRecordable
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "STAGE_INFO";

    [JsonPropertyName("currentStage")]
    public required Scheme.Stage CurrentStage { get; init; }

    [JsonPropertyName("totalTicks")]
    public required int TotalTicks { get; init; }
}
