using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Wall
{
    [JsonPropertyName("x")]
    public required int X { get; init; }

    [JsonPropertyName("y")]
    public required int Y { get; init; }

    [JsonPropertyName("angle")]
    public required double Angle { get; init; }
}
