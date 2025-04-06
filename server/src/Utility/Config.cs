using System.Text.Json.Serialization;

using Serilog;

namespace Thuai.Server.Utility;

/// <summary>
/// Configuration options for the server. Load from a file.
/// </summary>
public record Config
{

    [JsonPropertyName("server")]
    public ServerSettings Server { get; init; } = new();

    [JsonPropertyName("token")]
    public TokenSettings Token { get; init; } = new();

    [JsonPropertyName("log")]
    public LogSettings Log { get; init; } = new();

    [JsonPropertyName("game")]
    public GameSettings Game { get; init; } = new();

    [JsonPropertyName("recorder")]
    public RecorderSettings Recorder { get; init; } = new();

    /// <summary>
    /// The server settings.
    /// </summary>
    public record ServerSettings
    {
        [JsonPropertyName("port")]
        public int Port { get; init; } = 14514;
    }

    public record TokenSettings
    {
        [JsonPropertyName("loadTokenFromEnv")]
        public bool LoadTokenFromEnv { get; init; } = true;

        [JsonPropertyName("tokenLocation")]
        public string TokenLocation { get; init; } = "TOKENS";

        [JsonPropertyName("tokenDelimiter")]
        public char TokenDelimiter { get; init; } = ',';
    }

    /// <summary>
    /// The log settings.
    /// </summary>
    public record LogSettings
    {
        public enum LogTarget
        {
            Console,
            File,
            Both,
        }

        public enum LogLevel
        {
            Verbose,
            Debug,
            Information,
            Warning,
            Error,
            Fatal,
        }

        [JsonPropertyName("target")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogTarget Target { get; init; } = LogTarget.Console;

        [JsonPropertyName("minimumLevel")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogLevel MinimumLevel { get; init; } = LogLevel.Information;

        [JsonPropertyName("targetDirectory")]
        public string TargetDirectory { get; init; } = "./logs";

        [JsonPropertyName("rollingInterval")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RollingInterval RollingInterval { get; init; } = RollingInterval.Day;
    }

    /// <summary>
    /// The game settings.
    /// </summary>
    public record GameSettings()
    {
        [JsonPropertyName("ticksPerSecond")]
        public int TicksPerSecond { get; init; } = 20;

        /// <summary>
        /// Ticks for Buff Choosing.
        /// </summary>
        [JsonPropertyName("awardChooseTicks")]
        public int AwardChooseTicks { get; init; } = 2 * 20;

        /// <summary>
        /// Time limit per battle. (in ticks) 
        /// </summary>
        [JsonPropertyName("maxBattleTicks")]
        public int MaxBattleTicks { get; init; } = 60 * 20;

        /// <summary>
        /// The minimum player count to start a game.
        /// </summary>
        /// <remarks>
        /// Doesn't affect disconnection in game.
        /// </remarks>
        [JsonPropertyName("minimumPlayerCount")]
        public int MinimumPlayerCount { get; init; } = 2;

        /// <summary>
        /// Ticks to wait before the <see cref="GameLogic.Game"/> 
        /// goes into next stage, When the player count reaches 
        /// <see cref="MinimumPlayerCount"/>.
        /// </summary>
        [JsonPropertyName("playerWaitingTicks")]
        public int PlayerWaitingTicks { get; init; } = 10 * 20;

        [JsonPropertyName("battleCount")]
        public int BattleCount { get; init; } = 9;

        [JsonPropertyName("maxExtraBattleCount")]
        public int MaxExtraBattleCount { get; init; } = 1;
    }
    public record RecorderSettings()
    {

    }
}
