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
            Utility.Config config = Initialize();

            Connection.AgentServer agentServer = new() { Port = config.Server.Port };
            GameController.GameRunner gameRunner = new(config.Game);
            Recorder.Recorder recorder = new("./data", "replay.dat", "result.json");

            string[] tokens = [];

            try
            {
                tokens = Utility.Tools.TokenLoader.LoadTokens(config.Token);

                if (tokens.Length != tokens.Distinct().Count())
                {
                    _logger!.Warning("Duplicate tokens will be ignored.");
                    tokens = [.. tokens.Distinct()];
                }
            }
            catch (Exception)
            {
                _logger!.Error("Failed to load tokens. Please check your config file or token list.");
                _logger!.Error("Hint: If you are running this program for the first time,");
                _logger!.Error("      you may need to create a token file or set up environment variables.");
                _logger!.Error("You can change the token settings in the config file.");
                _logger!.Error("Press Ctrl+C to exit.");
                Task.Delay(-1).Wait();
            }

            gameRunner.Game.AddPlayer(tokens);

            // Subscribe to events
            gameRunner.Game.AfterGameTickEvent += agentServer.HandleAfterGameTickEvent;
            gameRunner.Game.AfterGameTickEvent += recorder.HandleAfterGameTickEvent;
            gameRunner.AfterPlayerConnectEvent += agentServer.HandleAfterPlayerConnectEvent;
            agentServer.AfterMessageReceiveEvent += gameRunner.HandleAfterMessageReceiveEvent;

            agentServer.Start();
            gameRunner.Start();

            while (gameRunner.IsRunning)
            {
                Task.Delay(1000).Wait();
            }

            recorder.Save();
            recorder.SaveResults(gameRunner.Game.Scoreboard);

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
                Environment.Exit(1);
            }

            _logger.Fatal($"Program crashed with exception:");
            _logger.Fatal(
                Utility.Tools.LogHandler.Truncate(
                    e.Message, Utility.Tools.LogHandler.MaximumMessageLength
                )
            );
            _logger.Fatal(e.StackTrace ?? "No stack trace available.");
            Environment.Exit(1);
        }
    }

    private static Utility.Config Initialize()
    {
        Utility.Config config = Utility.Tools.ConfigLoader.LoadOrCreateConfig(_configPath);

        Utility.Tools.LogHandler.Initialize(config.Log);
        Utility.EventChannel.Initialize();

        _logger = Utility.Tools.LogHandler.CreateLogger("Program");

        Version version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);

        _logger.Information("--------------------------------");
        _logger.Information($"THUAI8 Server v{version}");
        _logger.Information("Copyright (c) 2024-2025 THUASTA");
        _logger.Information("--------------------------------");

        return config;
    }
}
