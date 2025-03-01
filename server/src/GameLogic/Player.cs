using Serilog;
namespace Thuai.Server.GameLogic;

/// <summary>
/// Character controlled by a player.
/// </summary>
public partial class Player(string token, int playerId)
{
    public string Token => token;
    public int ID => playerId;
    public double Speed { get; set; } = Constants.MOVE_SPEED;

    public double TurnSpeed { get; set; } = Constants.TURN_SPEED;

    public Position PlayerPosition { get; set; } = new();

    public Weapon PlayerWeapon { get; set; } = new();

    public Armor PlayerArmor { get; set; } = new();

    public List<Skill> PlayerSkills { get; set; } = [];
    private readonly ILogger _logger = Log.ForContext("Component", $"Player {playerId}");

    public void Injured(int damage)
    {
        // TODO: Implement more complex logic for damage calculation.
        if (PlayerArmor.ArmorValue >= damage)
        {
            PlayerArmor.ArmorValue -= damage;
        }
        else if (PlayerArmor.ArmorValue < damage)
        {
            int realDamege = damage - PlayerArmor.ArmorValue;
            PlayerArmor.ArmorValue = 0;
            PlayerArmor.Health -= realDamege;
        }
    }

    public void Recover()
    {
        PlayerArmor.Recover();
        PlayerWeapon.Recover();
        foreach (Skill skill in PlayerSkills)
        {
            skill.Recover();
        }
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
        PlayerAttackEvent?.Invoke(this, new PlayerAttackEventArgs(this));
    }
}
