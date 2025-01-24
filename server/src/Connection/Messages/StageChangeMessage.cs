using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record StageChangeMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "STAGE_CHANGE";

    [JsonPropertyName("targetStage")]
    public string TargetStage { get; init; } = "";
}