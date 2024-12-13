using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record EnvironmentInfoMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "ENVIRONMENT_INFO";

    [JsonPropertyName("walls")]
    public List<Wall> Walls { get; init; } = new();

    [JsonPropertyName("fences")]
    public List<Fence> Fences { get; init; } = new();

    [JsonPropertyName("bullets")]
    public List<Bullet> Bullets { get; init; } = new();

    [JsonPropertyName("playerPositions")]
    public List<playerPositions> PlayerPositions { get; init; } = new();
}
