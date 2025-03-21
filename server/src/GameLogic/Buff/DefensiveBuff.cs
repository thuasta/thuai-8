namespace Thuai.Server.GameLogic.Buff;

public static class D_Buff
{
    public static void ARMOR(Player player)
    {
        // 护盾
        player.PlayerArmor.MaximumArmorValue += Constants.ARMOR_VALUE_INCREASE;
    }
    public static void REFLECT(Player player)
    {
        // 反弹
        player.PlayerArmor.MaximumArmorValue += Constants.ARMOR_VALUE_INCREASE;
        player.PlayerArmor.CanReflect = true;
    }
    public static void DODGE(Player player)
    {
        // 闪避
        player.PlayerArmor.DodgeRate += Constants.DODGE_PERCENTAGE_INCREASE;
    }
    public static void KNIFE(Player player)
    {
        // 名刀
        player.PlayerArmor.Knife = ArmorKnife.AVAILABLE;
    }
    public static void GRAVITY(Player player)
    {
        // 重力
        player.PlayerArmor.GravityField = true;
    }
}