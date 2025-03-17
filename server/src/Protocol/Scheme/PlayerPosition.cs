using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record PlayerPositions
{
    [JsonPropertyName("position")]
    public Position Position { get; init; } = new();

    [JsonPropertyName("token")]
    public string Token { get; init; } = "";
}