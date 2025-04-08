using nkast.Aether.Physics2D.Common;

using Serilog;

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
    public class Result(Player? winner, bool valid)
    {
        public Player? Winner { get; init; } = winner;
        public bool Valid { get; init; } = valid;
    }

    #region Fields and properties

    // <summary>
    /// Current tick of the battle.
    /// </summary>
    public int CurrentTick { get; private set; } = 0;

    public int AwardChoosingTickLimit { get; init; } = 0;

    /// <summary>
    /// Current Stage of the battle.
    /// </summary>
    public BattleStage Stage { get; private set; } = BattleStage.Waiting;

    /// <summary>
    /// Settings of the battle.
    /// </summary>
    public Utility.Config.GameSettings GameSettings { get; init; } = setting;

    public bool IsBattleOver => _currentBattleTick > GameSettings.MaxBattleTicks || AlivePlayers() <= 1;

    private int _currentAwardChoosingTick = 0;
    private int _currentBattleTick = 0;

    private readonly Physics.Environment _env = new();

    private readonly Random _random = new();
    private readonly ILogger _logger =
        Utility.Tools.LogHandler.CreateLogger("Battle");
    private readonly object _lock = new();

    #endregion

    #region Methods
    public Result GetResult()
    {
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
            lock (_lock)
            {
                bool success = GenerateMap();
                if (!success)
                {
                    throw new Exception("Generate Map Failed.");
                }

                BindWall(Map?.Walls ?? throw new Exception("Map is null."));
                _env.GenerateGrid(Map.Width, Map.Height);

                foreach (Player player in AllPlayers)
                {
                    player.InitializeWith(
                        _env.CreateBody(
                            Physics.Environment.Categories.Player,
                            Vector2.Zero,
                            0f
                        )
                    );

                    SubscribePlayerEvents(player);
                }
                ChooseSpawnpoint();
                UpdateGravityFieldCoverage();
                _logger.Information("Initialized battle successfully.");
                return true;
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Initialize battle failed:");
            Utility.Tools.LogHandler.LogException(_logger, e);
            return false;
        }
    }

    /// <summary>
    /// Runs one tick of the game.
    /// </summary>
    public void Tick()
    {
        _logger.Debug($"Running tick {CurrentTick} in battle. Current stage: {Stage}.");

        try
        {
            lock (_lock)
            {
                if (Stage == BattleStage.InBattle)
                {
                    RemoveOutsideObjects();

                    ActivatedLasers.Clear();
                    ActivateLasers(_lasersToActivate);
                    _lasersToActivate.Clear();

                    UpdatePlayerSpeed();

                    _env.Step();

                    UpdatePlayers();
                    UpdateBullets();
                    UpdateTraps();
                    UpdateMap();

                    ++_currentBattleTick;
                }
                else if (Stage == BattleStage.ChoosingAward)
                {
                    ActivatedLasers.Clear();    // To avoid recording error
                    ++_currentAwardChoosingTick;
                }
                StageControl();
                ++CurrentTick;
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Failed to run tick {CurrentTick}:");
            Utility.Tools.LogHandler.LogException(_logger, e);
        }
    }

    private void RemoveOutsideObjects()
    {
        if (Map is null)
        {
            throw new Exception("Map is null.");
        }

        foreach (Player player in AllPlayers)
        {
            if (!Map.IsInsideMap(player.PlayerPosition.Xpos, player.PlayerPosition.Ypos))
            {
                player.KillInstantly();
                _logger.Warning($"Player {player.ID} is outside the map and killed.");
            }
        }

        List<Bullet> bulletsToRemove = [];
        foreach (Bullet bullet in Bullets)
        {
            if (!Map.IsInsideMap(bullet.BulletPosition.Xpos, bullet.BulletPosition.Ypos))
            {
                bulletsToRemove.Add(bullet);
                _logger.Warning($"Bullet {bullet.Id} is outside the map and removed.");
            }
        }
        RemoveBullet(bulletsToRemove);

        List<Trap> trapsToRemove = [];
        foreach (Trap trap in Traps)
        {
            if (!Map.IsInsideMap(trap.TrapPosition.Xpos, trap.TrapPosition.Ypos))
            {
                trapsToRemove.Add(trap);
                _logger.Warning($"A trap is outside the map and removed.");
            }
        }
        RemoveTrap(trapsToRemove);
    }

    /// <summary>
    /// Control the stage of the battle.
    /// </summary>
    private void StageControl()
    {
        if (Stage == BattleStage.Waiting)
        {
            if (PlayerCount >= 2)
            {
                Stage = BattleStage.InBattle;
            }
        }
        else if (Stage == BattleStage.InBattle)
        {
            if (IsBattleOver)
            {
                foreach (Player player in AllPlayers)
                {
                    UnsubscribePlayerEvents(player);
                    player.LastChosenBuff = null;
                }
                Bullets.Clear();

                Stage = BattleStage.ChoosingAward;
            }
        }
        else if (Stage == BattleStage.ChoosingAward)
        {
            if (_currentAwardChoosingTick >= AwardChoosingTickLimit)
            {
                Stage = BattleStage.Finished;
            }
        }
        else /* Stage == BattleStage.Finished */
        {
            return;
        }
    }
    #endregion
}
