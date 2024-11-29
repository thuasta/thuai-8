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
    public bool isActive;

    public void PerformSkill(string skillName)
    {
        if (skillName == "BLACK_OUT")
        {

        }
        if (skillName == "SPEED_UP")
        {

        }
        if (skillName == "FLASH")
        {

        }
        if (skillName == "DESTROY")
        {

        }
        if (skillName == "CONSTRUCT")
        {

        }
        if (skillName == "TRAP")
        {

        }
        if (skillName == "MISSILE")
        {

        }
        if (skillName == "KAMUI")
        {

        }
    }
}
