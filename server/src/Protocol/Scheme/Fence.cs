using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

public record Fence
{
    [JsonPropertyName("position")]
    public Position Position { get; init; } = new();

    [JsonPropertyName("health")]
    public int Health { get; init; }
}