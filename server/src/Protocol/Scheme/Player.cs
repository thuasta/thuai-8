using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Player
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("weapon")]
    public required Weapon Weapon { get; init; }

    [JsonPropertyName("armor")]
    public required Armor Armor { get; init; }

    [JsonPropertyName("skills")]
    public required List<Skill> Skills { get; init; }

    [JsonPropertyName("position")]
    public required Position Position { get; init; }
}
