using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;
public record Bullet
{
    [JsonPropertyName("position")]
    public Position Position { get; init; } = new();

    [JsonPropertyName("speed")]
    public double Speed { get; init; }

    [JsonPropertyName("damage")]
    public double Damage { get; init; }

    [JsonPropertyName("traveledDistance")]
    public double TraveledDistance { get; init; }
}