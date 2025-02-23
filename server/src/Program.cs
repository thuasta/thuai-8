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

            // Just for prototype. Will be replaced by a real player adding system.
            gameRunner.Game.AddPlayer("token_1", 0);
            gameRunner.Game.AddPlayer("token_2", 1);
            gameRunner.Game.AddScore(gameRunner.Game.AllPlayers[0], 100);

            agentServer.Start();

            // Just for prototype. Will be replaced by a real game running system.
            Task.Delay(5000).Wait();

            // Just for prototype. Will be replaced by a recording system.
            recorder.Record(new Recorder.Error() { errorCode = 111111, message = "Damn the game isn't running" });
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

    private static Utility.Config Initialize()
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

        return config;
    }
}
