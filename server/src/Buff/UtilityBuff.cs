using Serilog.Debugging;
using Thuai.Server.GameLogic;

namespace Thuai.Server.Buff;


public static class U_Buff
{
    public static void BLACK_OUT(Tank tank)
    {
        // 视野限制
        tank.skills.Add(Skill("BLACK_OUT", 10, 0, false));
    }
    public static void SPEED_UP(Tank tank)
    {
        // 加速
        tank.skills.Add(Skill("SPEED_UP", 16, 0, false));
    }
    public static void FLASH(Tank tank)
    {
        // 闪现
        tank.skills.Add(Skill("FLASH", 25, 0, false));
    }
    public static void DESTROY(Tank tank)
    {
        // 破坏墙体
        tank.skills.Add(Skill("DESTROY", 18, 0, false));
    }
    public static void CONSTRUCT(Tank tank)
    {
        // 建造墙体
        tank.skills.Add(Skill("CONSTRUCT", 17, 0, false));
    }
    public static void TRAP(Tank tank)
    {
        // 陷阱
        tank.skills.Add(Skill("TRAP", 25, 0, false));
    }
    public static void MISSILE(Tank tank)
    {
        // 导弹
        tank.skills.Add(Skill("MISSILE", 30, 0, false));
    }
    public static void KAMUI(Tank tank)
    {
        // 虚化
        tank.skills.Add(Skill("KAMUI", 40, 0, false));
    }
}
