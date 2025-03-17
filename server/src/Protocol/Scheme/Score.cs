using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;
public record Score
{
    [JsonPropertyName("token")]
    public string Token { get; init; } = "";

    [JsonPropertyName("score")]
    public int score { get; init; } = 0;
}