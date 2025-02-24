namespace Thuai.Server.GameLogic.Buff;

public static class O_Buff
{
    public static void BULLET_COUNT(Player player)
    {
        // 子弹数量
        player.PlayerWeapon.maxBullets++;
    }
    public static void BULLET_SPEED(Player player)
    {
        // 子弹移速
        player.PlayerWeapon.bulletSpeed += 0.5;
    }
    public static void ATTACK_SPEED(Player player)
    {
        // 攻速
        player.PlayerWeapon.attackSpeed += 0.5;
    }
    public static void LASER(Player player)
    {
        // 激光
        player.PlayerWeapon.isLaser = true;
    }
    public static void DAMAGE(Player player)
    {
        // 伤害
        player.PlayerWeapon.damage += 10;
    }
    public static void ANTI_ARMOR(Player player)
    {
        // 破甲
        player.PlayerWeapon.antiArmor = true;
    }
}