using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;
public record Wall
{
    [JsonPropertyName("position")]
    public Position Position { get; init; } = new();
}