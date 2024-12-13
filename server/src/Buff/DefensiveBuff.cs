using Serilog.Debugging;
using Thuai.Server.GameLogic;

namespace Thuai.Server.Buff;

public static class D_Buff
{
    public static void ARMOR(Player player)
    {
        // 护盾
        player.PlayerArmor.armorValue++;
    }
    public static void REFLECT(Player player)
    {
        // 反弹
        player.PlayerArmor.armorValue++;
        player.PlayerArmor.canReflect = true;
    }
    public static void DODGE(Player player)
    {
        // 闪避
        player.PlayerArmor.dodgeRate += 0.1;
    }
    public static void KNIFE(Player player)
    {
        // 名刀
        player.PlayerArmor.knife = ArmorKnife.AVAILABLE;
    }
    public static void GRAVITY(Player player)
    {
        // 重力
        player.PlayerArmor.gravityField = true;
    }
}