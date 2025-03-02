namespace Thuai.Server.GameLogic.Buff;

public static class O_Buff
{
    public static void BULLET_COUNT(Player player)
    {
        // 子弹数量
        player.PlayerWeapon.MaxBullets += Constants.BULLETS_INCREASE;
    }
    public static void BULLET_SPEED(Player player)
    {
        // 子弹移速
        player.PlayerWeapon.BulletSpeed += Constants.BULLET_SPEED_INCREASE;
    }
    public static void ATTACK_SPEED(Player player)
    {
        // 攻速
        player.PlayerWeapon.AttackSpeed += Constants.ATTACK_SPEED_INCREASE;
    }
    public static void LASER(Player player)
    {
        // 激光
        player.PlayerWeapon.IsLaser = true;
    }
    public static void DAMAGE(Player player)
    {
        // 伤害
        player.PlayerWeapon.Damage += Constants.DAMAGE_INCREASE;
    }
    public static void ANTI_ARMOR(Player player)
    {
        // 破甲
        player.PlayerWeapon.AntiArmor = true;
    }
}