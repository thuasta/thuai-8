using System.Text.Json.Serialization;

namespace Thuai.GameServer.Recorder;

public record Result
{
    [JsonPropertyName("winner")]
    public required string Winner { get; init; }

    [JsonPropertyName("winnerPlayerId")]
    public required int WinnerPlayerId { get; init; }
    [JsonPropertyName("gameNumber")]
    public required int GameNumber { get; init; }
}