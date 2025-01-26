using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record Weapon
{
    [JsonPropertyName("attackSpeed")]
    public double AttackSpeed { get; init; }

    [JsonPropertyName("bulletSpeed")]
    public double BulletSpeed { get; init; }

    [JsonPropertyName("isLaser")]
    public bool IsLaser { get; init; }

    [JsonPropertyName("antiArmor")]
    public bool AntiArmor { get; init; }

    [JsonPropertyName("damage")]
    public int Damage { get; init; } = 0;

    [JsonPropertyName("maxBullets")]
    public int MaxBullets { get; init; } = 0;

    [JsonPropertyName("currentBullets")]
    public int CurrentBullets { get; init; } = 0;
}