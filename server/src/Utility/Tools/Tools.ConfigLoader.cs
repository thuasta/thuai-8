using System.Text.Json;

using Serilog;

namespace Thuai.Server.Utility;

public static partial class Tools
{

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
                    _logger.Warning($"Failed to load config file at {LogHandler.Truncate(path, 256)}");
                    LogHandler.LogException(_logger, e);
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

            Config config = new();
            string jsonString = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonString);
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
