namespace Thuai.Server.GameLogic;

public enum SkillName
{
    BLACK_OUT,
    SPEED_UP,
    FLASH,
    DESTROY,
    CONSTRUCT,
    TRAP,
    MISSILE,
    KAMUI
}

public interface ISkill
{
    public SkillName Name { get; }
    public int MaxCooldown { get; }
    public int CurrentCooldown { get; }
    public bool IsAvailable { get; }
    public bool IsActive { get; }
    public Player Owner { get; }

    public static SkillName SkillNameFromString(string skillName)
    {
        return (SkillName)Enum.Parse(typeof(SkillName), skillName);
    }

    public void Update();
    public void Recover();
    public void Activate();
    public void Deactivate();
}

public class BlackOut(int maxCooldown, int duration) : ISkill
{
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
    }

    public void Deactivate()
    {

    }
}
