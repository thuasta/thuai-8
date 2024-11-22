using System.Text.Json.Serialization;

namespace GameServer.Connection;

public record StageChange : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "STAGE_CHANGE";

    [JsonPropertyName("targetStage")]
    public string TargetStage { get; init; } = "";
}