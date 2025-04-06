namespace Thuai.Server.GameLogic.Skills;

public abstract class SkillWithDuration(int maxCooldown, int duration) : ISkill
{
    public event EventHandler<ISkill.OnActivationEventArgs>? OnActivationEvent = delegate { };
    public event EventHandler<ISkill.OnDeactivationEventArgs>? OnDeactivationEvent = delegate { };

    public abstract SkillName Name { get; }
    public int MaxCooldown => _cooldown.MaxCount;
    public int CurrentCooldown => _cooldown.CurrentCount;
    public bool IsAvailable => _cooldown.IsZero == true;
    public bool IsActive => _activation.IsZero == false;

    protected readonly Counter _cooldown = new(maxCooldown);
    protected readonly Counter _activation = new(duration);

    public virtual void Update()
    {
        _cooldown.Decrease();

        if (IsActive == true)
        {
            _activation.Decrease();

            if (IsActive == false)
            {
                Deactivate();
            }
        }
    }

    public void Reset()
    {
        Deactivate();
        _cooldown.Clear();
        _activation.Clear();
    }

    /// <summary>
    /// Recovers cooldown of the skill.
    /// Does not affect the activation state.
    /// </summary>
    public void Recover()
    {
        _cooldown.Clear();
    }

    public void Activate()
    {
        if (IsAvailable == false)
        {
            throw new InvalidOperationException("The skill is still in cooldown.");
        }

        _activation.Reset();
        _cooldown.Reset();
        OnActivationEvent?.Invoke(this, new(Name));
    }

    public void Deactivate()
    {
        _activation.Clear();
        OnDeactivationEvent?.Invoke(this, new(Name));
    }
}

public abstract class InstantSkill(int maxCooldown) : ISkill
{
    public event EventHandler<ISkill.OnActivationEventArgs>? OnActivationEvent = delegate { };
    public event EventHandler<ISkill.OnDeactivationEventArgs>? OnDeactivationEvent = delegate { };

    public abstract SkillName Name { get; }
    public int MaxCooldown => _cooldown.MaxCount;
    public int CurrentCooldown => _cooldown.CurrentCount;
    public bool IsAvailable => _cooldown.IsZero == true;
    public bool IsActive => false;  // Instant skills do not have an active state

    protected readonly Counter _cooldown = new(maxCooldown);

    public void Update()
    {
        _cooldown.Decrease();
    }

    public void Reset()
    {
        _cooldown.Clear();
    }

    public void Recover()
    {
        _cooldown.Clear();
    }

    public void Activate()
    {
        if (IsAvailable == false)
        {
            throw new InvalidOperationException("The skill is still in cooldown.");
        }

        _cooldown.Reset();
        OnActivationEvent?.Invoke(this, new(Name));
    }

    public void Deactivate()
    {
        // Do nothing for instant skills.
    }
}

public class BlackOut() : SkillWithDuration(Constants.SkillCooldown.BLACK_OUT, Constants.SkillDuration.BLACK_OUT)
{
    public override SkillName Name => SkillName.BLACK_OUT;
}

public class SpeedUp() : SkillWithDuration(Constants.SkillCooldown.SPEED_UP, Constants.SkillDuration.SPEED_UP)
{
    public override SkillName Name => SkillName.SPEED_UP;
}

public class Flash() : InstantSkill(Constants.SkillCooldown.FLASH)
{
    public override SkillName Name => SkillName.FLASH;
}

public class Destroy() : InstantSkill(Constants.SkillCooldown.DESTROY)
{
    public override SkillName Name => SkillName.DESTROY;
}

public class Construct() : InstantSkill(Constants.SkillCooldown.CONSTRUCT)
{
    public override SkillName Name => SkillName.CONSTRUCT;
}

public class Trap() : InstantSkill(Constants.SkillCooldown.TRAP)
{
    public override SkillName Name => SkillName.TRAP;
}

public class Recover() : InstantSkill(Constants.SkillCooldown.RECOVER)
{
    public override SkillName Name => SkillName.RECOVER;
}

public class Kamui() : SkillWithDuration(Constants.SkillCooldown.KAMUI, Constants.SkillDuration.KAMUI)
{
    public override SkillName Name => SkillName.KAMUI;

    public override void Update()
    {
        if (IsActive == true)
        {
            _activation.Decrease();

            // We don't cauculate cooldown because Kamui skill cauculates cooldown after effect ends.

            if (IsActive == false)
            {
                Deactivate();
            }
        }
        else
        {
            _cooldown.Decrease();
        }
    }
}
