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
    public record GameSettings(int TPS=20, int BCT=200, int MBT=2600)
    {
        /// <summary>
        /// Literally, ticks per second.
        /// </summary>
        public int TicksPerSecond { get; init; } = TPS;

        /// <summary>
        /// Ticks for Buff Choosing.
        /// </summary>
        public int BuffChooseTicks { get; init; } = BCT;

        /// <summary>
        /// Time limit per battle. (in ticks) 
        /// </summary>
        public int MaxBattleTicks { get; init; } = MBT; // 2600 = 20 * 120 + 200;

        

        // TODO: Implement
    }
}
