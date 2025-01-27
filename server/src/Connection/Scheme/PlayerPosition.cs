using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;
public record playerPositions
{
    [JsonPropertyName("position")]
    public Position Position { get; init; } = new();

    [JsonPropertyName("token")]
    public string Token { get; init; } = "";
}