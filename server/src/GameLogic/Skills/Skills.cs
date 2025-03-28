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

    private readonly Counter _cooldown = new(maxCooldown);
    private readonly Counter _activation = new(duration);

    public void Update()
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

    public void Recover()
    {
        _cooldown.Clear();
        _activation.Clear();
        Deactivate();
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

public class BlackOut() : SkillWithDuration(Constants.SkillCooldown.BLACK_OUT, Constants.SkillDuration.BLACK_OUT)
{
    public override SkillName Name => SkillName.BLACK_OUT;
}

public class SpeedUp() : SkillWithDuration(Constants.SkillCooldown.SPEED_UP, Constants.SkillDuration.SPEED_UP)
{
    public override SkillName Name => SkillName.SPEED_UP;
}
