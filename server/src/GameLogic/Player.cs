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

    public void Injured(int damage, bool antiArmor)
    {
        if (IsAlive == false)
        {
            _logger.Error("Cannot take damage: Player is already dead.");
            return;
        }

        // TODO: Implement more complex logic for damage calculation.
        if (_random.Next(0, 100) < PlayerArmor.DodgeRate)
        {
            // Dodged
            _logger.Information("Player dodged the attack.");
            return;
        }

        int realDamage = damage;
        if (antiArmor && PlayerArmor.ArmorValue > 0)
        {
            realDamage *= Constants.ANTI_ARMOR_FACTOR;
        }

        if (PlayerArmor.ArmorValue > 0)
        {
            // Damage absorbed by armor
            realDamage = Math.Min(realDamage, PlayerArmor.ArmorValue);
            PlayerArmor.ArmorValue -= realDamage;
            _logger.Information($"Armor absorbed {realDamage} damage.");
        }
        else
        {
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
    public void PlayerPerformSkill(SkillName skillName)
    {
        if (IsAlive == false)
        {
            _logger.Error("Failed to perform skill: Player is dead.");
            return;
        }
        if (PlayerSkills.Any(skill => skill.Name == skillName) == false)
        {
            _logger.Error($"Failed to perform skill: Player does not have the skill ({skillName}).");
            return;
        }
        if (PlayerSkills.First(skill => skill.Name == skillName).CurrentCooldown > 0)
        {
            _logger.Error($"Failed to perform skill: Skill ({skillName}) is on cooldown.");
            return;
        }

        // TODO: Check whether the player has can perform the skill or not.
        _logger.Information($"Perform skill ({skillName})");
        PlayerPerformSkillEvent?.Invoke(this, new PlayerPerformSkillEventArgs(this, skillName));
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
        if (IsAlive == false)
        {
            _logger.Error("Failed to attack: Player is dead.");
            return;
        }
        if (PlayerWeapon.CurrentBullets <= 0)
        {
            _logger.Error("Failed to attack: No bullets.");
            return;
        }

        // TODO: Check whether the player has can perform the attack or not.
        _logger.Information($"Attacking.");
        PlayerAttackEvent?.Invoke(this, new PlayerAttackEventArgs(this));
    }
}
