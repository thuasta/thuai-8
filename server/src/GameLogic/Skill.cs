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
    public int MaxCooldown = Constants.SKILL_MAX_COOLDOWN;
    public int CurrentCooldown;

    public bool IsReady => CurrentCooldown == 0;

    // TODO: Implement activation logic
    public bool IsActive => false;

    public static SkillName SkillNameFromString(String skill_name)
    {
        List<String> skillNames = [
            "BLACK_OUT", "SPEED_UP", "FLASH", "DESTROY", "CONSTRUCT", "TRAP", "MISSILE", "KAMUI"
        ];
        int SkillID = skillNames.IndexOf(skill_name);
        if (SkillID == -1)
        {
            throw new Exception("No such skills");
        }
        return (SkillName)SkillID;
    }

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

    public bool IsAvailable()
    {
        return CurrentCooldown == 0;
    }
}
