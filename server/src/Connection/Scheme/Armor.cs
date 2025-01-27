using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record Armor
{
    [JsonPropertyName("canReflect")]
    public bool CanReflect { get; init; }

    [JsonPropertyName("armorValue")]
    public int ArmorValue { get; init; } = 0;

    [JsonPropertyName("health")]
    public int Health { get; init; } = 0;

    [JsonPropertyName("gravityField")]
    public bool GravityField { get; init; }

    [JsonPropertyName("knife")]
    public string Knife { get; init; } = "";

    [JsonPropertyName("dodgeRate")]
    public double DodgeRate { get; init; }
}
