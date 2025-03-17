using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Skill
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("maxCooldown")]
    public int MaxCooldown { get; init; } = 0;

    [JsonPropertyName("currentCooldown")]
    public int CurrentCooldown { get; init; } = 0;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }
}
