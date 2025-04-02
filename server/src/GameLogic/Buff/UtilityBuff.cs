namespace Thuai.Server.GameLogic.Buff;

public static class U_Buff
{
    public static void AppendSkill(Player player, ISkill skill)
    {
        skill.OnActivationEvent += player.OnSkillActivation;
        skill.OnDeactivationEvent += player.OnSkillDeactivation;
        player.PlayerSkills.Add(skill);
    }

    public static void BLACK_OUT(Player player)
    {
        // 视野限制
        AppendSkill(player, new Skills.BlackOut());
        player.LastChosenBuff = Buff.BLACK_OUT;
    }

    public static void SPEED_UP(Player player)
    {
        // 加速
        AppendSkill(player, new Skills.SpeedUp());
        player.LastChosenBuff = Buff.SPEED_UP;
    }

    public static void FLASH(Player player)
    {
        // 闪现
        AppendSkill(player, new Skills.Flash());
        player.LastChosenBuff = Buff.FLASH;
    }

    public static void DESTROY(Player player)
    {
        // 破坏墙体
        AppendSkill(player, new Skills.Destroy());
        player.LastChosenBuff = Buff.DESTROY;
    }

    public static void CONSTRUCT(Player player)
    {
        // 建造墙体
        AppendSkill(player, new Skills.Construct());
        player.LastChosenBuff = Buff.CONSTRUCT;
    }

    public static void TRAP(Player player)
    {
        // 陷阱
        AppendSkill(player, new Skills.Trap());
        player.LastChosenBuff = Buff.TRAP;
    }

    public static void RECOVER(Player player)
    {
        AppendSkill(player, new Skills.Recover());
        player.LastChosenBuff = Buff.RECOVER;
    }

    public static void KAMUI(Player player)
    {
        // 虚化
        AppendSkill(player, new Skills.Kamui());
        player.LastChosenBuff = Buff.KAMUI;
    }
}
