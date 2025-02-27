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
    public SkillName name = skillName;
    public int maxCooldown;
    public int currentCooldown;

    public void UpdateCoolDown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown--;
        }
    }
}
