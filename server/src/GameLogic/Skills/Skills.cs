namespace Thuai.Server.GameLogic;

public class BlackOut(
    int maxCooldown = Constants.SkillCooldown.BLACK_OUT,
    int duration = Constants.SkillDuration.BLACK_OUT
) : ISkill
{
    public event EventHandler<ISkill.OnActivationEventArgs>? OnActivationEvent = delegate { };
    public event EventHandler<ISkill.OnDeactivationEventArgs>? OnDeactivationEvent = delegate { };

    public SkillName Name => SkillName.BLACK_OUT;
    public int MaxCooldown => _cooldown.MaxCount;
    public int CurrentCooldown => _cooldown.CurrentCount;
    public bool IsAvailable => _cooldown.IsZero == true;
    public bool IsActive => _activation.IsZero == false;
    public required Player Owner { get; init; }

    private readonly Counter _cooldown = new(maxCooldown);
    private readonly Counter _activation = new(duration);

    public void Update()
    {
        _cooldown.Decrease();
        _activation.Decrease();

        if (IsActive == false)
        {
            Deactivate();
        }
    }

    public void Recover()
    {
        _cooldown.Clear();
        _activation.Clear();
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
        OnDeactivationEvent?.Invoke(this, new(Name));
    }
}
