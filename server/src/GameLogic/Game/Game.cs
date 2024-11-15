using Serilog;
using Thuai.Server.GameController;

namespace Thuai.Server.GameLogic;

/// <summary>
/// Represents the game logic.
/// </summary>
public partial class Game(Utility.Config.GameSettings gameSettings)
{
    public enum GameStage
    {
        Waiting,
        PreparingGame,
        PreparingBattle,
        InBattle,
        Finished
    }

    #region Fields and properties
    /// <summary>
    /// The config of the game.
    /// </summary>
    public Utility.Config.GameSettings GameSettings { get; } = gameSettings;

    /// <summary>
    /// The current running battle.
    /// </summary>
    public Battle? RunningBattle { get; private set; } = null;

    private readonly ILogger _logger = 
        Utility.Tools.LogHandler.CreateLogger("Game");
    private readonly object _lock = new();

    public GameStage Stage { get; private set; } = GameStage.Waiting;

    #endregion

    #region Methods
    
    // TODO: Add properties

    /// <summary>
    /// Initializes the game.
    /// </summary>
    public void Initialize()
    {
        try
        {

        }
        catch (Exception e)
        {
            _logger.Error($"Failed to initialize the game: {e.Message}");
            _logger.Debug($"{e}");
        }
    }

    /// <summary>
    /// Ticks the game. Called every tick.
    /// </summary>
    /// <remarks>
    /// <see cref="Game"/> itself doesn't keep track of its tick, it's only 
    /// meaningful for <see cref="Battle"/>. But something still needs to 
    /// be run every tick for <see cref="Game"/>.
    /// </remarks>
    public void Tick()
    {
        try
        {
            lock(_lock)
            {

            }
        }
        catch (Exception e)
        {
            _logger.Error($"An exception occurred while ticking the game: {e.Message}");
            _logger.Debug($"{e}");
        }
    }

    /// <summary>
    /// Controls the battles.
    /// </summary>
    private void BattleControl()
    {

    }

    /// <summary>
    /// Controls the Stage.
    /// </summary>
    private void StageControl()
    {

    }

    #endregion

}
