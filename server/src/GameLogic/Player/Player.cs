using nkast.Aether.Physics2D.Dynamics;
using Serilog;

namespace Thuai.Server.GameLogic;

/// <summary>
/// Character controlled by a player.
/// </summary>
public partial class Player(string token, int playerId)
{
    public string Token => token;
    public string TruncatedToken => Utility.Tools.LogHandler.Truncate(Token, 8);
    public string RecordToken => (playerId + 1).ToString(); // Because client actually reads ID and it starts from 1 ...
    public int ID => playerId;

    public float MaxSpeed => Constants.MAXIMUM_MOVE_SPEED;
    public float MaxTurnSpeed => Constants.MAXIMUM_TURN_SPEED;
    public float Speed { get; set; } = Constants.MAXIMUM_MOVE_SPEED;
    public float TurnSpeed { get; set; } = Constants.MAXIMUM_TURN_SPEED;

    public MoveDirection MoveDirection { get; set; } = MoveDirection.NONE;
    public TurnDirection TurnDirection { get; set; } = TurnDirection.NONE;

    public Position PlayerPosition
    {
        get
        {
            if (Body is null)
            {
                _logger.Error("Player is not bound to a body.");
                return new();
            }
            return new(Body.Position.X, Body.Position.Y, Body.Rotation);
        }
        set
        {
            if (Body is null)
            {
                _logger.Error("Player is not bound to a body.");
                return;
            }
            Body.Position = new(value.Xpos, value.Ypos);
            Body.Rotation = value.Angle;
            float velocity = Body.LinearVelocity.Length();
            Body.LinearVelocity = new(
                (float)(velocity * Math.Cos(value.Angle)), (float)(velocity * Math.Sin(value.Angle))
            );
        }
    }

    public Weapon PlayerWeapon { get; set; } = new();
    public Armor PlayerArmor { get; set; } = new();
    public List<ISkill> PlayerSkills { get; set; } = [];
    public Buff.Buff? LastChosenBuff { get; set; } = null;

    public bool HasChosenAward => LastChosenBuff is not null;
    public bool IsAlive => PlayerArmor.Health > 0;

    private readonly Random _random = new();

    private readonly ILogger _logger = Log.ForContext("Component", $"Player {playerId}");

    public void InitializeWith(Body body)
    {
        Bind(body);

        if (PlayerArmor.GravityField == true)
        {
            AppendGravityField();
        }
        Reset();
    }

    public void Update()
    {
        PlayerArmor.Knife.Update();
        PlayerWeapon.Update();
        _stunCounter.Decrease();
        foreach (ISkill skill in PlayerSkills)
        {
            skill.Update();
        }
    }

    public void UpdateSpeed()
    {
        if (Body is null)
        {
            _logger.Error("Cannot update speed: player is not bound to a body.");
            return;
        }

        Physics.Tag tag = (Physics.Tag)Body.Tag;
        float linear = IsStunned ? 0f : MoveDirection switch
        {
            MoveDirection.NONE => 0f,
            MoveDirection.BACK => -(float)tag.AttachedData[Physics.Key.SpeedUpFactor] * Speed,
            MoveDirection.FORTH => (float)tag.AttachedData[Physics.Key.SpeedUpFactor] * Speed,
            _ => throw new ArgumentException("Unknown move direction.")
        };
        float angular = IsStunned ? 0f : TurnDirection switch
        {
            TurnDirection.NONE => 0f,
            TurnDirection.CLOCKWISE => -(float)tag.AttachedData[Physics.Key.SpeedUpFactor] * TurnSpeed,
            TurnDirection.COUNTER_CLOCKWISE => (float)tag.AttachedData[Physics.Key.SpeedUpFactor] * TurnSpeed,
            _ => throw new ArgumentException("Unknown turn direction.")
        };

        if (IsInvulnerable == false && (int)tag.AttachedData[Physics.Key.CoveredFields] > 0)
        {
            linear *= Constants.GRAVITY_FIELD_STRENGTH;
            angular *= Constants.GRAVITY_FIELD_STRENGTH;
        }

        Body.LinearVelocity = Orientation * linear;
        Body.AngularVelocity = angular;
    }

    /// <summary>
    /// Kill the player instantly.
    /// Used when the player is out of the map.
    /// </summary>
    public void KillInstantly()
    {
        _logger.Warning("KillInstantly is only called when invalid behavior is detected.");
        PlayerArmor.Health = 0;
    }

    public void Injured(int damage, bool antiArmor, out bool reflected)
    {
        reflected = false;

        if (damage < 0)
        {
            _logger.Error("Damage is negative. Please contact the developer.");
            return;
        }
        if (IsAlive == false)
        {
            _logger.Error("Cannot take damage: Player is already dead.");
            return;
        }

        if (IsInvulnerable == true)
        {
            // Invulnerability
            _logger.Information("Player is invulnerable.");
            return;
        }

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

            if (PlayerArmor.CanReflect == true)
            {
                // The bullet will be reflected
                reflected = true;
                _logger.Information("Armor reflected the bullet.");
            }
        }
        else
        {
            if (realDamage >= PlayerArmor.Health && PlayerArmor.Knife.IsAvailable == true)
            {
                PlayerArmor.Knife.Activate();
                _stunCounter.Clear();       // Stun effect is removed
                PlayerArmor.Health = Constants.REMAINING_HEALTH_VALUE;
                _logger.Information("Invulnerability invoked by taking damage.");
            }
            else
            {
                PlayerArmor.Health -= realDamage;
                _logger.Information($"Player took {realDamage} damage.");
                if (PlayerArmor.Health <= 0)
                {
                    _logger.Information("Player died.");
                }
            }
        }
    }

    public void Reset()
    {
        PlayerArmor.Recover();
        PlayerWeapon.Recover();
        _stunCounter.Clear();
        foreach (ISkill skill in PlayerSkills)
        {
            skill.Reset();
        }
        _logger.Information("Reset succeed.");
    }

    /// <summary>
    /// Recover the player and all its skills, except the Recover skill itself.
    /// Only used by Recover skill.
    /// </summary>
    private void Recover()
    {
        if (IsAlive == false)
        {
            _logger.Error("Failed to recover: Player is dead.");
            return;
        }

        PlayerArmor.Recover();
        _stunCounter.Clear();   // Stun effect is removed

        // Weapon does not recover automatically

        foreach (ISkill skill in PlayerSkills)
        {
            if (skill.Name == SkillName.RECOVER)
            {
                // Do not recover the skill itself
                continue;
            }
            skill.Recover();
        }
        _logger.Information("Recovered.");
    }

    /// <summary>
    /// Publish a skill event.
    /// </summary>
    /// <param name="skillName">The type of the skill.</param>
    public void PlayerPerformSkill(SkillName skillName)
    {
        if (IsAlive == false)
        {
            _logger.Debug("Failed to perform skill: Player is dead.");
            return;
        }

        ISkill? skill = PlayerSkills.Find(skill => skill.Name == skillName);
        if (skill is null)
        {
            _logger.Debug($"Failed to perform skill: Player does not have the skill ({skillName}).");
            return;
        }
        if (skill.IsAvailable == false)
        {
            _logger.Debug($"Failed to perform skill: Skill ({skillName}) is on cooldown.");
            return;
        }

        try
        {
            _logger.Information($"Performing skill ({skillName})");

            EndKamuiEffect();

            skill.Activate();
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to perform skill ({skillName}):");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }

    public void PlayerAttack()
    {
        if (IsAlive == false)
        {
            _logger.Debug("Failed to attack: Player is dead.");
            return;
        }
        if (PlayerWeapon.HasEnoughBullets == false)
        {
            _logger.Debug("Failed to attack: No bullets.");
            return;
        }
        if (PlayerWeapon.CanAttack == false)
        {
            _logger.Debug("Failed to attack: Weapon is on cooldown.");
            return;
        }

        _logger.Information($"Attacking.");

        EndKamuiEffect();

        // Laser does not consume bullets because it activates instantly
        if (PlayerWeapon.IsLaser == false)
        {
            --PlayerWeapon.CurrentBullets;
        }
        PlayerWeapon.Reset();

        PlayerAttackEvent?.Invoke(this, new PlayerAttackEventArgs(this));
    }

    public void EndKamuiEffect()
    {
        if (Kamui == false)
        {
            return;
        }

        ISkill? kamui = PlayerSkills.Find(skill => skill.Name == SkillName.KAMUI);
        if (kamui is null || kamui is not Skills.Kamui || kamui.IsActive == false)
        {
            return;
        }

        kamui.Deactivate();
        _logger.Information("Kamui effect interrupted by another operation.");
    }
}
