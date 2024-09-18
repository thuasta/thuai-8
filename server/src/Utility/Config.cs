using System.Text.Json.Serialization;

namespace Thuai.Server.Utility;

/// <summary>
/// Configuration options for the server. Load from a file.
/// </summary>
public record Config
{
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
    }

    /// <summary>
    /// The game settings.
    /// </summary>
    public record GameSettings
    {
        // TODO: Implement
    }
}
