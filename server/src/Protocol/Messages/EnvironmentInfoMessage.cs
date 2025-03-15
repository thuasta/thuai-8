using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record EnvironmentInfoMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "ENVIRONMENT_INFO";

    [JsonPropertyName("walls")]
    public List<Scheme.Wall> Walls { get; init; } = [];

    [JsonPropertyName("fences")]
    public List<Scheme.Fence> Fences { get; init; } = [];

    [JsonPropertyName("bullets")]
    public List<Scheme.Bullet> Bullets { get; init; } = [];

    [JsonPropertyName("mapSize")]
    public int MapSize { get; init; } = 100;
}
