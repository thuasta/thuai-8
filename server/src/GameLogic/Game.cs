using Serilog;

namespace Thuai.Server.GameLogic;

/// <summary>
/// Represents the game logic.
/// </summary>
public class Game
{
    /// <summary>
    /// Current tick of the game.
    /// </summary>
    public int CurrentTick { get; private set; } = 0;

    // TODO: Add properties

    private readonly ILogger _logger = Utility.Tools.LogHandler.CreateLogger("Game");
    private readonly object _lock = new();

    /// <summary>
    /// Initializes the game.
    /// </summary>
    public void Initialize()
    {
        _logger.Information("Initializing game...");

        // TODO: Implement

        _logger.Information("Game initialized.");
    }

    /// <summary>
    /// Runs one tick of the game.
    /// </summary>
    public void Tick()
    {
        try
        {
            lock (_lock)
            {
                // TODO: Implement game logic here
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Failed to run tick {CurrentTick}:");
            Utility.Tools.LogHandler.LogException(_logger, e);
        }
    }
}
