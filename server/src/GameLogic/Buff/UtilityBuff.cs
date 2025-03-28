namespace Thuai.Server.GameLogic.Buff;

public static class U_Buff
{
    public static void BLACK_OUT(Player player)
    {
        // 视野限制
        Skills.BlackOut blackOut = new();
        blackOut.OnActivationEvent += player.OnSkillActivation;
        blackOut.OnDeactivationEvent += player.OnSkillDeactivation;
        player.PlayerSkills.Add(blackOut);
        player.LastChosenBuff = Buff.BLACK_OUT;
    }

    public static void SPEED_UP(Player player)
    {
        // 加速
        // TODO: Implement
        player.LastChosenBuff = Buff.SPEED_UP;
    }

    public static void FLASH(Player player)
    {
        // 闪现
        player.LastChosenBuff = Buff.FLASH;
    }

    public static void DESTROY(Player player)
    {
        // 破坏墙体
        player.LastChosenBuff = Buff.DESTROY;
    }

    public static void CONSTRUCT(Player player)
    {
        // 建造墙体
        player.LastChosenBuff = Buff.CONSTRUCT;
    }

    public static void TRAP(Player player)
    {
        // 陷阱
        player.LastChosenBuff = Buff.TRAP;
    }

    public static void MISSILE(Player player)
    {
        // 导弹
        player.LastChosenBuff = Buff.MISSILE;
    }

    public static void KAMUI(Player player)
    {
        // 虚化
        player.LastChosenBuff = Buff.KAMUI;
    }
}
