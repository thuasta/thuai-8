using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

public record Fence
{
    [JsonPropertyName("position")]
    public required Position Position { get; init; } = new();

    [JsonPropertyName("health")]
    public required int Health { get; init; }
}