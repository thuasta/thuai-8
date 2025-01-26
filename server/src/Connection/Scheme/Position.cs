using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;
public record Position
{
    [JsonPropertyName("x")]
    public double X { get; init; }

    [JsonPropertyName("y")]
    public double Y { get; init; }

    [JsonPropertyName("angle")]
    public double Angle { get; init; }
}