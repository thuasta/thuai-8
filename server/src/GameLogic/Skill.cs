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
    public int maxActiveTime;
    public int currentActiveTime;

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

    public void Start()
    {

    }
    public void Activate()
    {
        currentActiveTime = maxActiveTime;
        Start();
    }

    public void End()
    {

    }

    public void UpdateActiveTime()
    {
        if (currentActiveTime > 0)
        {
            currentActiveTime--;
            if (currentActiveTime == 0)
            {
                End();
            }
        }
    }
}
