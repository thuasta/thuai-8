using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

public record Armor
{
    [JsonPropertyName("canReflect")]
    public required bool CanReflect { get; init; }

    [JsonPropertyName("armorValue")]
    public required int ArmorValue { get; init; }

    [JsonPropertyName("health")]
    public required int Health { get; init; }

    [JsonPropertyName("gravityField")]
    public required bool GravityField { get; init; }

    [JsonPropertyName("knife")]
    public required string Knife { get; init; }

    [JsonPropertyName("dodgeRate")]
    public required double DodgeRate { get; init; }
}
