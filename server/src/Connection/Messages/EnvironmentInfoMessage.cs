using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record EnvironmentInfoMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "ENVIRONMENT_INFO";

    [JsonPropertyName("walls")]
    public List<Wall> Walls { get; init; } = [];

    [JsonPropertyName("fences")]
    public List<Fence> Fences { get; init; } = [];

    [JsonPropertyName("bullets")]
    public List<Bullet> Bullets { get; init; } = [];

    [JsonPropertyName("mapSize")]
    public int MapSize { get; init; } = 100;
}
