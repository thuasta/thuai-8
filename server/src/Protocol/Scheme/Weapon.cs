using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

public record Weapon
{
    [JsonPropertyName("attackSpeed")]
    public required double AttackSpeed { get; init; }

    [JsonPropertyName("bulletSpeed")]
    public required double BulletSpeed { get; init; }

    [JsonPropertyName("isLaser")]
    public required bool IsLaser { get; init; }

    [JsonPropertyName("antiArmor")]
    public required bool AntiArmor { get; init; }

    [JsonPropertyName("damage")]
    public required int Damage { get; init; }

    [JsonPropertyName("maxBullets")]
    public required int MaxBullets { get; init; }

    [JsonPropertyName("currentBullets")]
    public required int CurrentBullets { get; init; }
}