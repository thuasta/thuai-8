namespace Thuai.Server.GameLogic.Buff;

public static class U_Buff
{
    public static void BLACK_OUT(Player player)
    {
        // 视野限制
        BlackOut blackOut = new();
        blackOut.OnActivationEvent += player.OnSkillActivation;
        blackOut.OnDeactivationEvent += player.OnSkillDeactivation;
        player.PlayerSkills.Add(blackOut);
    }

    public static void SPEED_UP(Player player)
    {
        // 加速

    }

    public static void FLASH(Player player)
    {
        // 闪现

    }

    public static void DESTROY(Player player)
    {
        // 破坏墙体

    }

    public static void CONSTRUCT(Player player)
    {
        // 建造墙体

    }

    public static void TRAP(Player player)
    {
        // 陷阱

    }

    public static void MISSILE(Player player)
    {
        // 导弹

    }

    public static void KAMUI(Player player)
    {
        // 虚化

    }
}
