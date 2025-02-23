using System.Text.Json.Serialization;

namespace Thuai.Server.Recorder;

public record Result
{
    [JsonPropertyName("scores")]
    public required Dictionary<string, int> Scores { get; init; } = [];
}
