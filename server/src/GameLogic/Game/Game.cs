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
    public Utility.Config.GameSettings GameSettings { get; init; } = gameSettings;

    /// <summary>
    /// The current running battle.
    /// </summary>
    public Battle? RunningBattle { get; private set; } = null;
    public int BattleNumber { get; private set; } = 0;
    public GameStage Stage { get; private set; } = GameStage.Waiting;

    public int WaitingTick { get; private set; } = 0;

    private readonly ILogger _logger = 
        Utility.Tools.LogHandler.CreateLogger("Game");
    private readonly object _lock = new();

    #endregion

    #region Methods

    /// <summary>
    /// Initializes the game.
    /// </summary>
    public void Initialize()
    {
        try
        {
            // TODO:
            // 1. Generate the order of awards.
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
                RunningBattle?.Tick();
                StageControl();
                // TODO: implement, but maybe not anymore?
            }
        }
        catch (Exception e)
        {
            _logger.Error($"An exception occurred while ticking the game: {e.Message}");
            _logger.Debug($"{e}");
        }
    }

    /// <summary>
    /// Controls the Stage and Battles.
    /// </summary>
    private void StageControl()
    {
        if (Stage == GameStage.Waiting)
        {
            if (PlayerCount >= GameSettings.MinimumPlayerCount)
            {
                ++WaitingTick;
                if (WaitingTick >= GameSettings.PlayerWaitingTicks)
                {
                    Stage = GameStage.PreparingGame;
                }
            }
        } 
        else if (Stage == GameStage.PreparingGame)
        {
            Initialize();
            Stage = GameStage.PreparingBattle;
        } 
        else if (Stage == GameStage.PreparingBattle)
        {
            if (BattleNumber < GameSettings.BattleCount || NeedAdditionalBattle())
            {
                RunningBattle = new Battle(GameSettings, AllPlayers);
                Stage = GameStage.InBattle;
            }
            else 
            {
                Stage = GameStage.Finished;
            }
        }
        else if (Stage == GameStage.InBattle)
        {
            if (RunningBattle is null)
            {
                throw new Exception("Battle doesn't exist.");
            }
            else if (RunningBattle.Stage == Battle.BattleStage.Finished)
            {
                Player? winner = RunningBattle.GetResult().Winner;
                if (winner is not null)
                {
                    ++Scoreboard[winner];
                }
                RunningBattle = null;
                ++BattleNumber;
                Stage = GameStage.PreparingBattle;
            }
        }
        else /* Stage == GameStage.Finished */
        {
            // TODO: implement.
        }
    }

    private bool NeedAdditionalBattle()
    {
        int highScore = 0;
        foreach (Player player in AllPlayers)
        {
            if (Scoreboard[player] > highScore)
            {
                highScore = Scoreboard[player];
            }
        }
        int highScoreCount = 0;
        foreach (Player player in AllPlayers)
        {
            if (Scoreboard[player] == highScore)
            {
                ++highScoreCount;
            }
        }
        return highScoreCount != 1;
    }

    #endregion

}
