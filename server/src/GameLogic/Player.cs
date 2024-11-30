using Serilog;
namespace Thuai.Server.GameLogic;

/// <summary>
/// Character controlled by a player.
/// </summary>
public partial class Player
{
    public string Token { get; set; }
    public int ID { get; set; }
    public double Speed { get; set; }

    public Position PlayerPosition { get; set; } = new();

    public Weapon PlayerWeapon { get; set; } = new();

    public Armor PlayerArmor { get; set; } = new();

    public List<Skill> PlayerSkills { get; set; } = [];
    private readonly ILogger _logger;

    public Player(string token, int playerId)
    {
        Token = token;
        ID = playerId;

        _logger = Log.ForContext("Component", $"Player {playerId}");
    }


    public void PlayerMove(MoveDirection direction)
    {
        _logger.Debug($"Move to ({direction}).");
        PlayerMoveEvent?.Invoke(this, new PlayerMoveEventArgs(this, direction));
    }

    public void PlayerTurn(TurnDirection direction)
    {
        _logger.Debug($"Turn in ({direction}).");
        PlayerTurnEvent?.Invoke(this, new PlayerTurnEventArgs(this, direction));
    }

    public void PlayerAttack()
    {
        _logger.Debug($"Player attack.");
        PlayerWeapon.SubBullet();
        PlayerAttackEvent?.Invoke(this, new PlayerAttackEventArgs(this));
    }
}



