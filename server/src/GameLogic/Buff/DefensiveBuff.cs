namespace Thuai.Server.GameLogic.Buff;

public static class D_Buff
{
    public static void ARMOR(Player player)
    {
        // 护盾
        player.PlayerArmor.MaximumArmorValue += Constants.ARMOR_VALUE_INCREASE;
        player.LastChosenBuff = Buff.ARMOR;
    }
    public static void REFLECT(Player player)
    {
        // 反弹
        player.PlayerArmor.MaximumArmorValue += Constants.ARMOR_VALUE_INCREASE;
        player.PlayerArmor.CanReflect = true;
        player.LastChosenBuff = Buff.REFLECT;
    }
    public static void DODGE(Player player)
    {
        // 闪避
        player.PlayerArmor.DodgeRate += Constants.DODGE_PERCENTAGE_INCREASE;
        player.LastChosenBuff = Buff.DODGE;
    }
    public static void KNIFE(Player player)
    {
        // 名刀
        player.PlayerArmor.Knife.Acquire();
        player.LastChosenBuff = Buff.KNIFE;
    }
    public static void GRAVITY(Player player)
    {
        // 重力
        player.PlayerArmor.GravityField = true;
        player.LastChosenBuff = Buff.GRAVITY;
    }
}
