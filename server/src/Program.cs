using System.Reflection;
using Serilog;

namespace Thuai.Server;

public class Program
{
    static readonly ILogger _logger = Utility.Tools.LogHandler.CreateLogger("Program");

    public static void Main(string[] args)
    {
        try
        {
            Utility.Tools.LogHandler.Initialize(
                Utility.Tools.LogHandler.DefaultLogSettings
            );

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
            _logger.Fatal(e.Message);
            _logger.Fatal(e.StackTrace ?? "No stack trace available.");
        }
    }
}
