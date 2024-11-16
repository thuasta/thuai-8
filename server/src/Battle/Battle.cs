using Serilog;
using Thuai.Server.GameController;

namespace Thuai.Server.GameLogic;

/// <summary>
/// Simulates a round of game.
/// </summary>
public partial class Battle(Utility.Config.GameSettings setting, bool buff) 
{
    /// <summary>
    /// Represents the result of a battle.
    /// </summary>
    /// <remarks>
    /// !Valid means game not end, "Winner is null" means a draw.
    /// </remarks>
    public enum BattleStage 
    {
        Waiting,
        ChoosingBuff,
        InBattle,
        Finished
    }
    
    public class Result(Player? winner, bool valid) {
        public Player? Winner { get; init; } = winner;
        public bool Valid { get; init; } = valid;
    }
    #region Fields and properties

    // <summary>
    /// Current tick of the game.
    /// </summary>
    public int CurrentTick { get; private set; } = 0;

    /// <summary>
    /// Current Stage of the Game.
    /// </summary>
    public BattleStage Stage { get; private set;} = BattleStage.Waiting;

    /// <summary>
    /// Settings of the game.
    /// </summary>
    public required Utility.Config.GameSettings GameSettings { get; init; } = setting;
    
    private readonly ILogger _logger = 
        Utility.Tools.LogHandler.CreateLogger("Battle");

    private readonly object _lock = new();

    #endregion 

    #region Methods
    public Result GetResult() {
        if (Stage != BattleStage.Finished)
        {
            return new Result(null, false);
        }
        else 
        {
            // TODO: Get the winner and return.
            return new(null, true);
        }
    }

    public bool IsRunning() {
        return false;
    }
    private bool NeedToChooseBuff { get; init; } = buff;

    /// <summary>
    /// Initialize the battle.
    /// </summary>
    /// <exception cref="Exception">Generate Map Failed.</exception>
    public void Initialize() {
        _logger.Information("Initializing game...");

        if (!GenerateMap())
        {
            throw new Exception("Generate Map Failed");
        }

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
                // TODO: Ticks here.
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Failed to run tick {CurrentTick}:");
            Utility.Tools.LogHandler.LogException(_logger, e);
        }
    }

    /// <summary>
    /// Control the stage of the battle.
    /// </summary>
    private void StageControl() {
        if (Stage == BattleStage.Waiting) 
        {
            
        }
        else if (Stage == BattleStage.ChoosingBuff) 
        {

        } 
        else if (Stage == BattleStage.InBattle) 
        {

        } 
        else /* Stage == BattleStage.Finished */
        {

        }
    }
    #endregion
}
