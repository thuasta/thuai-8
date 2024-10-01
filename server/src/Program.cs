using System.Reflection;
using Serilog;

namespace Thuai.Server;

public class Program
{
    private static readonly ILogger _logger = Utility.Tools.LogHandler.CreateLogger("Program");

    private static readonly string _configPath = "config/config.json";

    public static void Main(string[] args)
    {
        try
        {
            // Initialize logger
            Utility.Tools.LogHandler.Initialize(Utility.Tools.LogHandler.DefaultLogSettings);

            Utility.Config config = Utility.Tools.ConfigLoader.LoadOrCreateConfig(_configPath);

            Version version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);

            _logger.Information("--------------------------------");
            _logger.Information($"THUAI8 Server v{version}");
            _logger.Information("Copyright (c) 2024 THUASTA");
            _logger.Information("--------------------------------");

            // TODO: Implement

        }
        catch (Exception e)
        {
            _logger.Fatal($"Program crashed with exception:");
            _logger.Fatal(
                Utility.Tools.LogHandler.Truncate(
                    e.Message, Utility.Tools.LogHandler.MaximumMessageLength
                )
            );
            _logger.Fatal(e.StackTrace ?? "No stack trace available.");
        }
    }
}
