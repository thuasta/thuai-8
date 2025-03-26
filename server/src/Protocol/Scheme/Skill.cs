using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Skill
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("maxCooldown")]
    public required int MaxCooldown { get; init; }

    [JsonPropertyName("currentCooldown")]
    public required int CurrentCooldown { get; init; }

    [JsonPropertyName("isActive")]
    public required bool IsActive { get; init; }
}
