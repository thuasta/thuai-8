using Thuai.Server.Connection;

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
    public SkillName Name { get; init; }

    public int CurrentCooldown { get; }
    public int MaxCooldown { get; init; }

    public bool IsActive { get; }

    public static SkillName SkillNameFromString(string skill_name)
    {
        return (SkillName)Enum.Parse(typeof(SkillName), skill_name);
    }

    public void Update();

    public void Recover();

    public bool IsAvailable();

    public void Perform();
}

public class GeneralContinuousSkill(
    SkillName skillName, 
    int maxCoolDown=Constants.SKILL_MAX_COOLDOWN,
    int skillDuration=Constants.SKILL_DURATION
) : ISkill
{
    public SkillName Name { get; init; } = skillName;

    public int CurrentCooldown { get; private set; } = 0;
    public int RemainingActivationTicks { get; private set; } = 0;

    public int MaxCooldown { get; init; } = maxCoolDown;
    public int SkillDuration { get; init; } = skillDuration;

    public bool IsActive => RemainingActivationTicks > 0;

    public bool IsAvailable()
    {
        return CurrentCooldown > 0;

    }

    public void Perform()
    {
        CurrentCooldown = MaxCooldown;
        RemainingActivationTicks = SkillDuration;
    }

    public void Recover()
    {
        CurrentCooldown = 0;
        RemainingActivationTicks = 0;
    }

    public void Update()
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown--;
        }
        if (RemainingActivationTicks > 0)
        {
            RemainingActivationTicks--;
        }
    }
}

public class InstantSkill(
    SkillName skillName, 
    int maxCoolDown=Constants.SKILL_MAX_COOLDOWN
) : ISkill
{
    public SkillName Name { get; init; } = skillName;

    public int CurrentCooldown { get; private set; } = 0;
    public int MaxCooldown { get; init; } = maxCoolDown;

    public bool IsActive => false;

    public bool IsAvailable()
    {
        return CurrentCooldown > 0;
    }

    public void Perform()
    {
        CurrentCooldown = MaxCooldown;
    }

    public void Recover()
    {
        CurrentCooldown = 0;
    }

    public void Update()
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown--;
        }
    }
}

public class SkillMissile(SkillName skillName) : ISkill
{
    public SkillName Name { get; init; } = skillName;

    public int CurrentCooldown => throw new NotImplementedException();

    public int MaxCooldown { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

    bool ISkill.IsActive => throw new NotImplementedException();

    public bool IsAvailable()
    {
        throw new NotImplementedException();
    }

    public void Perform()
    {
        throw new NotImplementedException();
    }

    public void Recover()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}

public class SkillKamui(SkillName skillName) : ISkill
{
    public SkillName Name { get; init; } = skillName;

    public int CurrentCooldown => throw new NotImplementedException();

    public int MaxCooldown { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

    bool ISkill.IsActive => throw new NotImplementedException();
    
    public bool IsAvailable()
    {
        throw new NotImplementedException();
    }

    public void Perform()
    {
        throw new NotImplementedException();
    }

    public void Recover()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}