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

    private readonly Random _random = new();

    private readonly ILogger _logger = Log.ForContext("Component", $"Player {playerId}");

    public void Injured(int damage)
    {
        // TODO: Implement more complex logic for damage calculation.
        if (_random.Next(0, 100) < PlayerArmor.DodgeRate)
        {
            _logger.Information("Player dodged the attack.");
            return;
        }

        if (PlayerArmor.ArmorValue >= damage)
        {
            PlayerArmor.ArmorValue -= damage;
            _logger.Information($"Armor absorbed {damage} damage.");
        }
        else if (PlayerArmor.ArmorValue < damage)
        {
            int realDamage = damage - PlayerArmor.ArmorValue;
            _logger.Information($"Armor absorbed {PlayerArmor.ArmorValue} damage.");
            PlayerArmor.ArmorValue = 0;

            if (realDamage >= PlayerArmor.Health && PlayerArmor.Knife == ArmorKnife.AVAILABLE)
            {
                // TODO: Set activation interval
                PlayerArmor.Knife = ArmorKnife.BROKEN;
                realDamage = PlayerArmor.Health - 1;
                _logger.Debug("Invulnerability invoked by taking damage.");
            }

            PlayerArmor.Health -= realDamage;
            _logger.Information($"Player took {realDamage} damage.");
            if (PlayerArmor.Health <= 0)
            {
                _logger.Information("Player died.");
            }
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
        _logger.Information("Recovered.");
    }

    /// <summary>
    /// Publish a skill event.
    /// </summary>
    /// <param name="skill_name">The type of the skill.</param>
    public void PlayerPerformSkill(SkillName skill_name)
    {
        // TODO: Check whether the player has can perform the skill or not.
        _logger.Information($"Perform skill ({skill_name})");
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
        // TODO: Check whether the player has can perform the attack or not.
        _logger.Information($"Attacking.");
        PlayerAttackEvent?.Invoke(this, new PlayerAttackEventArgs(this));
    }
}
