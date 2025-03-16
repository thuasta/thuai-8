using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

public record Trap
{
    [JsonPropertyName("position")]
    public required PositionInt Position { get; init; }

    [JsonPropertyName("isActive")]
    public required bool IsActive { get; init; }
}