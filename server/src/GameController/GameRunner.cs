using Serilog;

namespace Thuai.Server.GameController;

/// <summary>
/// Runs and manages the game.
/// </summary>
public class GameRunner(Utility.Config.GameSettings gameSettings)
{
    public Utility.Config.GameSettings GameSettings = gameSettings;

    private bool _isRunning = false;
    private Utility.ClockProvider _clockProvider = new(gameSettings.TicksPerSecond);
    
    private readonly ILogger _logger = Utility.Tools.LogHandler.CreateLogger("GameRunner");

    public void Start()
    {
        Task.Run(RunGame);
    }

    private void RunGame()
    {
        if (_isRunning)
        {
            _logger.Warning("Cannot run a running game again.");
            return;
        }
        GameLogic.Game game = new(GameSettings);

        game.Initialize();

        _isRunning = true;
        while (_isRunning)
        {
            Task clock = _clockProvider.CreateClock();
            game.Tick();

            if (game.Stage == GameLogic.Game.GameStage.Finished)
            {
                _isRunning = false;
            }

            clock.Wait();
        }
    }
}
