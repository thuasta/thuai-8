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

public class Skill
{
    public SkillName name;
    public int maxCooldown;
    public int currentCooldown;

    public Skill(SkillName skillName)
    {
        name = skillName;
    }

    public void UpdateCoolDown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown--;
        }
    }
}
