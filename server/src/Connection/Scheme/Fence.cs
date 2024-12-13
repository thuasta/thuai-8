using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record Fence
    {
        [JsonPropertyName("position")]
        public Position Position { get; init; } = new();

        [JsonPropertyName("health")]
        public int Health { get; init; }
    }