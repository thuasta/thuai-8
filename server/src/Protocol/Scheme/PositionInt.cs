using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

public record PositionInt
{
    [JsonPropertyName("x")]
    public required int X { get; init; }

    [JsonPropertyName("y")]
    public required int Y { get; init; }

    [JsonPropertyName("angle")]
    public required int Angle { get; init; }
}
