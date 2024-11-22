using System.Text.Json.Serialization;

namespace GameServer.Connection;

public record GameStatistics : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "GAME_STATISTICS";

    [JsonPropertyName("currentStage")]
    public string CurrentStage { get; init; } = "";

    [JsonPropertyName("countDown")]
    public int CountDown { get; init; } = 0;

    [JsonPropertyName("ticks")]
    public int Ticks { get; init; } = 0;

    [JsonPropertyName("scores")]
    public List<Score> Scores { get; init; } = new();

    public record scores
    {
        [JsonPropertyName("token")]
        public string Token { get; init; } = "";

        [JsonPropertyName("score")]
        public int Score { get; init; } = 0;
    }

}
