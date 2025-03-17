using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Laser
{
    [JsonPropertyName("start")]
    public Position Start { get; init; } = new();

    [JsonPropertyName("end")]
    public Position End { get; init; } = new();
}