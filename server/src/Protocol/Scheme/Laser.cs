using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Laser
{
    [JsonPropertyName("start")]
    public required Position Start { get; init; } = new();

    [JsonPropertyName("end")]
    public required Position End { get; init; } = new();
}