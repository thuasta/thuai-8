using Serilog;
using Thuai.Server.GameController;


namespace Thuai.Server.GameLogic;

/// <summary>
/// Simulates a round of game.
/// </summary>
public partial class Battle(Utility.Config.GameSettings setting, List<Player> players) 
{
    public enum BattleStage 
    {
        Waiting,
        InBattle,
        ChoosingAward,
        Finished
    }
    
    /// <summary>
    /// Represents the result of a battle.
    /// </summary>
    /// <remarks>
    /// !Valid means game not end, "Winner is null" means a draw.
    /// </remarks>
    public class Result(Player? winner, bool valid) {
        public Player? Winner { get; init; } = winner;
        public bool Valid { get; init; } = valid;
    }

    #region Fields and properties

    // <summary>
    /// Current tick of the battle.
    /// </summary>
    public int CurrentTick { get; private set; } = 0;

    /// <summary>
    /// Current Stage of the battle.
    /// </summary>
    public BattleStage Stage { get; private set;} = BattleStage.Waiting;

    /// <summary>
    /// Settings of the battle.
    /// </summary>
    public Utility.Config.GameSettings GameSettings { get; init; } = setting;
    
    private readonly ILogger _logger = 
        Utility.Tools.LogHandler.CreateLogger("Battle");

    private readonly object _lock = new();

    #endregion 

    #region Methods
    public Result GetResult() {
        if (Stage != BattleStage.Finished && Stage != BattleStage.ChoosingAward)
        {
            return new Result(null, false);
        }
        else 
        {
            return new(PlayerWithHighestHP(), true);
        }
    }

    /// <summary>
    /// Initialize the battle.
    /// </summary>
    public bool Initialize() 
    {
        _logger.Information("Initializing battle...");
        try
        {
            bool success = GenerateMap();
            if (!success)
            {
                throw new Exception("Generate Map Failed");
            }
            ChooseSpawnpoint();
            _logger.Information("Initialized battle successfully.");
            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Initialize battle failed: {e.Message}");
            _logger.Debug($"{e}");
            return false;
        }
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
                if (Stage == BattleStage.InBattle)
                {
                    UpdatePlayers();
                    UpdateBullets();
                    UpdateMap();
                    ++CurrentTick;
                }
                StageControl();
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Failed to run tick {CurrentTick}:");
            Utility.Tools.LogHandler.LogException(_logger, e);
        }
    }

    public bool IsBattleOver() {
        return CurrentTick > GameSettings.MaxBattleTicks || AlivePlayers() <= 1;
    }

    /// <summary>
    /// Control the stage of the battle.
    /// </summary>
    private void StageControl() {
        if (Stage == BattleStage.Waiting) 
        {
            if (PlayerCount >= 2)
            {
                Stage = BattleStage.InBattle;
            }
        }
        else if (Stage == BattleStage.InBattle) 
        {
            if (IsBattleOver())
            {
                Stage = BattleStage.ChoosingAward;
            }
        } 
        else if (Stage == BattleStage.ChoosingAward) 
        {
            // TODO: implement.
        } 
        else /* Stage == BattleStage.Finished */
        {
            return;
        }
    }
    #endregion
}
