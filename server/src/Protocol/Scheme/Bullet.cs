using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Bullet
{
    [JsonPropertyName("no")]
    public required int No { get; init; }

    [JsonPropertyName("isMissile")]
    public bool IsMissile { get; init; }

    [JsonPropertyName("isAntiArmor")]
    public bool IsAntiArmor { get; init; }

    [JsonPropertyName("position")]
    public required Position Position { get; init; }

    [JsonPropertyName("speed")]
    public required double Speed { get; init; }

    [JsonPropertyName("damage")]
    public required double Damage { get; init; }

    [JsonPropertyName("traveledDistance")]
    public required double TraveledDistance { get; init; }
}