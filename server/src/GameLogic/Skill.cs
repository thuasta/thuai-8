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

public class Skill(SkillName skillName)
{
    public SkillName Name { get; init; } = skillName;
    public int MaxCooldown => Constants.SKILL_MAX_COOLDOWN;
    public int CurrentCooldown;

    public bool IsReady => CurrentCooldown == 0;

    public void UpdateCoolDown()
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown--;
        }
    }

    public void Recover()
    {
        CurrentCooldown = 0;
    }
}
