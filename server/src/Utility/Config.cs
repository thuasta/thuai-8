using System.Text.Json.Serialization;

using Serilog;
using static Thuai.Server.GameLogic.Game;

namespace Thuai.Server.Utility;

/// <summary>
/// Configuration options for the server. Load from a file.
/// </summary>
public record Config
{

    [JsonPropertyName("server")]
    public ServerSettings Server { get; init; } = new();

    [JsonPropertyName("log")]
    public LogSettings Log { get; init; } = new();

    [JsonPropertyName("game")]
    public GameSettings Game { get; init; } = new();

    /// <summary>
    /// The server settings.
    /// </summary>
    public record ServerSettings
    {
        // TODO: Implement
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
    public record GameSettings(int TPS=20, int ACT=200, int MBT=2400, int MPC=2, int PWT=200, int BC=9)
    {
        /// <summary>
        /// Literally, ticks per second.
        /// </summary>
        public int TicksPerSecond { get; init; } = TPS;

        /// <summary>
        /// Ticks for Buff Choosing.
        /// </summary>
        public int AwardChooseTicks { get; init; } = ACT;

        /// <summary>
        /// Time limit per battle. (in ticks) 
        /// </summary>
        public int MaxBattleTicks { get; init; } = MBT; // 2400 = 20 * 120;

        /// <summary>
        /// The minimum player count to start a game.
        /// </summary>
        /// <remarks>
        /// Doesn't affect disconnection in game.
        /// </remarks>
        public int MinimumPlayerCount { get; init; } = MPC; 

        /// <summary>
        /// Ticks to wait before the <see cref="GameLogic.Game"/> 
        /// goes into next stage, When the player count reaches 
        /// <see cref="MinimumPlayerCount"/>.
        /// </summary>
        public int PlayerWaitingTicks = PWT;

        public int BattleCount = BC;

        // TODO: Implement
    }
}
