using Serilog;
using Thuai.Server.GameLogic.Buff;

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
    public int AwardCount => GameSettings.BattleCount - 1;
    public GameStage Stage { get; private set; } = GameStage.Waiting;
    public int WaitingTick { get; private set; } = 0;
    public Buff.Buff[] AvilableBuffsAfterCurrentBattle { get; private set; } = new Buff.Buff[3];

    private BuffSelector _buffSelector = new();

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
            _logger.Information("Game initialized.");
        }
        catch (Exception e)
        {
            _logger.Error($"Failed to initialize the game:");
            Utility.Tools.LogHandler.LogException(_logger, e);
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
        _logger.Debug("Running a new tick.");
        _logger.Debug($"Current stage: {Stage}");
        try
        {
            lock (_lock)
            {
                RunningBattle?.Tick();
                StageControl();
            }
            AfterGameTickEvent?.Invoke(this, new AfterGameTickEventArgs(this));
        }
        catch (Exception e)
        {
            _logger.Error($"An exception occurred while ticking the game:");
            Utility.Tools.LogHandler.LogException(_logger, e);
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
            Stage = GameStage.PreparingBattle;
        }
        else if (Stage == GameStage.PreparingBattle)
        {
            if (BattleNumber < AwardCount)
            {
                AvilableBuffsAfterCurrentBattle = _buffSelector.ShowBuff(BattleNumber + 1);
            }

            if (BattleNumber < GameSettings.BattleCount || NeedAdditionalBattle())
            {
                RunningBattle = new Battle(GameSettings, AllPlayers)
                {
                    AwardChoosingTickLimit = GameSettings.AwardChooseTicks
                };
                RunningBattle.Initialize();
                Stage = GameStage.InBattle;
                _logger.Information($"Battle {BattleNumber} started.");
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
                throw new Exception($"Battle {BattleNumber} doesn't exist.");
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
        // return GetHighScorePlayer() == null;
        return false;
    }

    #endregion

}
