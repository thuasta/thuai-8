using Serilog.Debugging;
using Thuai.Server.GameLogic;

namespace Thuai.Server.Buff;


public static class U_Buff
{
    public static void BLACK_OUT(Player player)
    {
        // 视野限制
        player.PlayerSkills.Add(new Skill(SkillName.BLACK_OUT));
    }

    public static void SPEED_UP(Player player)
    {
        // 加速
        player.PlayerSkills.Add(new Skill(SkillName.SPEED_UP));
    }

    public static void FLASH(Player player)
    {
        // 闪现
        player.PlayerSkills.Add(new Skill(SkillName.FLASH));
    }

    public static void DESTROY(Player player)
    {
        // 破坏墙体
        player.PlayerSkills.Add(new Skill(SkillName.DESTROY));
    }

    public static void CONSTRUCT(Player player)
    {
        // 建造墙体
        player.PlayerSkills.Add(new Skill(SkillName.CONSTRUCT));
    }

    public static void TRAP(Player player)
    {
        // 陷阱
        player.PlayerSkills.Add(new Skill(SkillName.TRAP));
    }

    public static void MISSILE(Player player)
    {
        // 导弹
        player.PlayerSkills.Add(new Skill(SkillName.MISSILE));
    }

    public static void KAMUI(Player player)
    {
        // 虚化
        player.PlayerSkills.Add(new Skill(SkillName.KAMUI));
    }
}
