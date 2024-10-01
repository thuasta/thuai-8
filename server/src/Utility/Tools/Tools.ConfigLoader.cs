using System.Text.Json;

using Serilog;

namespace Thuai.Server.Utility;

public static partial class Tools
{
    private static readonly JsonSerializerOptions _jsOptions = new()
    {
        WriteIndented = true,
    };

    private static readonly ILogger _logger = LogHandler.CreateLogger("ConfigLoader");

    public static class ConfigLoader
    {
        /// <summary>
        /// Load or create a configuration file.
        /// </summary>
        /// <param name="path">Path of config file(e.g. "config/config.json")</param>
        public static Config LoadOrCreateConfig(string path)
        {
            if (File.Exists(path) == false)
            {
                _logger.Warning(
                    $"Config file not found at {LogHandler.Truncate(path, 256)}. Creating new config file."
                );
                return CreateConfig(path);
            }
            else
            {
                try
                {
                    return LoadConfig(path);
                }
                catch (Exception e)
                {
                    _logger.Error($"Failed to load config file at {LogHandler.Truncate(path, 256)}");
                    LogHandler.LogException(_logger, e);
                    _logger.Error("Creating new config file.");

                    return CreateConfig(path);
                }
            }
        }

        /// <summary>
        /// Create a configuration file.
        /// </summary>
        /// <param name="path">Path of config file(e.g. "config/config.json")</param>
        public static Config CreateConfig(string path)
        {
            if (File.Exists(path) == true)
            {
                _logger.Warning($"Config file already exists at {LogHandler.Truncate(path, 256)}. Overwriting.");
            }

            string directory =
                Path.GetDirectoryName(path) ??
                throw new Exception($"Failed to get directory name from {LogHandler.Truncate(path, 256)}.");

            if ((string.IsNullOrEmpty(directory) == false) && (Directory.Exists(directory) == false))
            {
                Directory.CreateDirectory(directory);
                _logger.Debug($"Directory created at {LogHandler.Truncate(directory, 256)}.");
            }

            Config config = new();
            string jsonString = JsonSerializer.Serialize(config, _jsOptions);
            File.WriteAllText(path, jsonString);

            _logger.Information($"Config file created at {LogHandler.Truncate(path, 256)}.");

            return config;
        }

        public static Config LoadConfig(string path)
        {
            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException("Config file not found.", path);
            }

            string jsonString = File.ReadAllText(path);
            Config config =
                JsonSerializer.Deserialize<Config>(jsonString) ??
                throw new Exception("Failed to deserialize config file.");
            return config;
        }
    }
}
