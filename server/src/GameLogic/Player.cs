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

    public MoveDirection MoveDirection { get; set; } = MoveDirection.NONE;
    public TurnDirection TurnDirection { get; set; } = TurnDirection.NONE;

    public Position PlayerPosition { get; set; } = new();

    public Weapon PlayerWeapon { get; set; } = new();

    public Armor PlayerArmor { get; set; } = new();

    public List<ISkill> PlayerSkills { get; set; } = [];
    public bool HasChosenAward { get; set; } = false;

    public bool IsAlive => PlayerArmor.Health > 0;

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
            int realDamage = damage - PlayerArmor.ArmorValue;
            PlayerArmor.ArmorValue = 0;
            PlayerArmor.Health -= realDamage;
        }
    }

    public void Recover()
    {
        PlayerArmor.Recover();
        PlayerWeapon.Recover();
        foreach (ISkill skill in PlayerSkills)
        {
            skill.Recover();
        }
        HasChosenAward = false;
    }

    /// <summary>
    /// Publish a skill event.
    /// </summary>
    /// <param name="skill_name">The type of the skill.</param>
    public void PlayerPerformSkill(SkillName skill_name)
    {
        _logger.Debug($"Perform skill ({skill_name})");
        PlayerPerformSkillEvent?.Invoke(this, new PlayerPerformSkillEventArgs(this, skill_name));
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
