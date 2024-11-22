using System.Text.Json;

namespace Thuai.Server.Utility;

public static partial class Tools
{

    /// <summary>
    /// A class for loading and creating configurations.
    /// </summary>
    public static class ConfigLoader
    {
        private static readonly JsonSerializerOptions _jsOptions = new()
        {
            WriteIndented = true,
        };

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
                catch (Exception)
                {
                    if (File.Exists(path) == true)
                    {
                        File.Delete(path);
                    }
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
                throw new Exception($"Config file already exists at {LogHandler.Truncate(path, 256)}.");
            }

            string directory =
                Path.GetDirectoryName(path) ??
                throw new Exception($"Failed to get directory name from {LogHandler.Truncate(path, 256)}.");

            if ((string.IsNullOrEmpty(directory) == false) && (Directory.Exists(directory) == false))
            {
                Directory.CreateDirectory(directory);
            }

            Config config = new();
            string jsonString = JsonSerializer.Serialize(config, _jsOptions);
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
