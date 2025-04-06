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

    public int CurrentTick { get; private set; } = 0;

    /// <summary>
    /// The current running battle.
    /// </summary>
    public Battle? RunningBattle { get; private set; } = null;
    public int BattleNumber { get; private set; } = 0;
    public int AwardCount => GameSettings.BattleCount - 1;
    public GameStage Stage { get; private set; } = GameStage.Waiting;
    public int WaitingTick { get; private set; } = 0;
    public List<Buff.Buff> AvailableBuffsAfterCurrentBattle { get; private set; } = [];
    public BuffSelector BuffSelector { get; private set; } = new();

    public bool HasAwardBeforeBattle => BattleNumber > 0 && BattleNumber <= AwardCount;
    public bool HasAwardAfterBattle => BattleNumber < AwardCount;

    private readonly Random _random = new();

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
        _logger.Debug($"Running tick {CurrentTick}. Current stage: {Stage}.");
        try
        {
            lock (_lock)
            {
                RunningBattle?.Tick();
                StageControl();
                ++CurrentTick;
            }
            AfterGameTickEvent?.Invoke(this, new AfterGameTickEventArgs(this));
        }
        catch (Exception e)
        {
            _logger.Error($"An exception occurred while running tick {CurrentTick}:");
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
            if (HasAwardBeforeBattle)
            {
                // Have buffs before the current battle.
                foreach (Player player in AllPlayers)
                {
                    if (player.LastChosenBuff is null)
                    {
                        // Select a buff for the player.
                        BuffSelector.SelectBuff(player, _random.Next(1, BuffSelector.BUFF_KINDS + 1));
                    }
                    _logger.Information($"[Player {player.ID}] Last buff: {player.LastChosenBuff}.");
                }
            }

            if (HasAwardAfterBattle)
            {
                AvailableBuffsAfterCurrentBattle = [.. BuffSelector.ShowBuff(BattleNumber + 1)];
            }
            else
            {
                AvailableBuffsAfterCurrentBattle = [];
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
        if (BattleNumber >= GameSettings.BattleCount + GameSettings.MaxExtraBattleCount)
        {
            return false;
        }

        return GetHighScorePlayer() == null;
    }

    #endregion

}
