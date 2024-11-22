using Serilog.Debugging;
using Thuai.Server.GameLogic;

namespace Thuai.Server.Buff;

public static class D_Buff
{
    public static void ARMOR(Tank tank)
    {
        // 护盾
        tank.TankArmor.armorValue++;
    }
    public static void REFLECT(Tank tank)
    {
        // 反弹
        tank.TankArmor.armorValue++;
        tank.TankArmor.canReflect = true;
    }
    public static void DODGE(Tank tank)
    {
        // 闪避
        tank.TankArmor.dodgeRate += 0.1
    }
    public static void KNIFE(Tank tank)
    {
        // 名刀
        tank.TankArmor.knife = "AVAILABLE";
    }
    public static void GRAVITY(Tank tank)
    {
        // 重力
        tank.TankArmor.gravityField = true;
    }
}