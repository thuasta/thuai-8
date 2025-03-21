using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Wall
{
    [JsonPropertyName("position")]
    public Position Position { get; init; } = new();
}