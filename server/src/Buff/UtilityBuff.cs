using Serilog.Debugging;
using Thuai.Server.GameLogic;

namespace Thuai.Server.Buff;


public static class U_Buff
{
    public static void BLACK_OUT(Tank tank)
    {
        // 视野限制
        tank.TankSkills.Add(new Skill
        {
            name = SkillName.BLACK_OUT,
            maxCooldown = 10,
            currentCooldown = 0,
            isActive = false
        });
    }

    public static void SPEED_UP(Tank tank)
    {
        // 加速
        tank.TankSkills.Add(new Skill
        {
            name = SkillName.SPEED_UP,
            maxCooldown = 16,
            currentCooldown = 0,
            isActive = false
        });
    }

    public static void FLASH(Tank tank)
    {
        // 闪现
        tank.TankSkills.Add(new Skill
        {
            name = SkillName.FLASH,
            maxCooldown = 25,
            currentCooldown = 0,
            isActive = false
        });
    }

    public static void DESTROY(Tank tank)
    {
        // 破坏墙体
        tank.TankSkills.Add(new Skill
        {
            name = SkillName.DESTROY,
            maxCooldown = 18,
            currentCooldown = 0,
            isActive = false
        });
    }

    public static void CONSTRUCT(Tank tank)
    {
        // 建造墙体
        tank.TankSkills.Add(new Skill
        {
            name = SkillName.CONSTRUCT,
            maxCooldown = 17,
            currentCooldown = 0,
            isActive = false
        });
    }

    public static void TRAP(Tank tank)
    {
        // 陷阱
        tank.TankSkills.Add(new Skill
        {
            name = SkillName.TRAP,
            maxCooldown = 25,
            currentCooldown = 0,
            isActive = false
        });
    }

    public static void MISSILE(Tank tank)
    {
        // 导弹
        tank.TankSkills.Add(new Skill
        {
            name = SkillName.MISSILE,
            maxCooldown = 30,
            currentCooldown = 0,
            isActive = false
        });
    }

    public static void KAMUI(Tank tank)
    {
        // 虚化
        tank.TankSkills.Add(new Skill
        {
            name = SkillName.KAMUI,
            maxCooldown = 40,
            currentCooldown = 0,
            isActive = false
        });
    }
}
