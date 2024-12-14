using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;
public record Laser {
            [JsonPropertyName("start")]
            public Position start { get; init; } = new();

            [JsonPropertyName("end")]
            public Position End { get; init; } = new();
        }