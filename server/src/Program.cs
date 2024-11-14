using System.Reflection;
using Serilog;

namespace Thuai.Server;

public class Program
{
    private static ILogger? _logger = null;    // Should be created after LogHandler.Initialize

    private static readonly string _configPath = "config/config.json";

    public static void Main(string[] args)
    {
        try
        {
            Initialize();

            // TODO: Implement

            throw new NotImplementedException();

        }
        catch (Exception e)
        {
            if (_logger is null)
            {
                Console.WriteLine("FATAL: Program crashed with exception:");
                Console.WriteLine(
                    Utility.Tools.LogHandler.Truncate(
                        e.Message, Utility.Tools.LogHandler.MaximumMessageLength
                    )
                );
                Console.WriteLine(e.StackTrace ?? "No stack trace available.");
                return;
            }

            _logger.Fatal($"Program crashed with exception:");
            _logger.Fatal(
                Utility.Tools.LogHandler.Truncate(
                    e.Message, Utility.Tools.LogHandler.MaximumMessageLength
                )
            );
            _logger.Fatal(e.StackTrace ?? "No stack trace available.");
        }
    }

    private static void Initialize()
    {
        Utility.Config config = Utility.Tools.ConfigLoader.LoadOrCreateConfig(_configPath);

        Utility.Tools.LogHandler.Initialize(config.Log);
        Utility.EventChannel.Initialize();

        _logger = Utility.Tools.LogHandler.CreateLogger("Program");

        Version version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);

        _logger.Information("--------------------------------");
        _logger.Information($"THUAI8 Server v{version}");
        _logger.Information("Copyright (c) 2024 THUASTA");
        _logger.Information("--------------------------------");
    }
}
