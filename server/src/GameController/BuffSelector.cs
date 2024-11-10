using Serilog.Debugging;

namespace Thuai.Server.GameController;

/// <summary>
/// Selects a buff for a player.
/// </summary>

public class BuffSelector
{
    /// <summary>
    /// Buffs that can be selected.
    /// </summary>
    public enum Buff
    {
        BULLET_COUNT, // 子弹数量
        BULLET_SPEED, // 子弹移速
        ATTACK_SPEED, // 攻速
        LASER, // 激光
        DAMAGE, // 伤害
        ANTI_ARMOR, // 破甲
        ARMOR, // 护盾
        REFLECT, // 反弹
        DODGE, // 闪避
        KNIFE, // 名刀
        GRAVITY, // 重力
        BLACK_OUT, // 视野限制
        SPEED_UP, // 加速
        FLASH, // 闪现
        DESTROY, // 破坏墙体
        CONSTRUCT, // 建造墙体
        TRAP, // 陷阱
        MISSILE, // 导弹
        KAMUI, // 虚化
    }

    /// <summary>
    /// Three types of buffs.
    /// </summary>
    private Buff[] OffensiveBuff = new Buff[]
    {
        Buff.BULLET_COUNT,
        Buff.BULLET_COUNT,
        Buff.BULLET_SPEED,
        Buff.ATTACK_SPEED,
        Buff.ATTACK_SPEED,
        Buff.DAMAGE,
        Buff.LASER,
        Buff.ANTI_ARMOR
    };
    private Buff[] DefensiveBuff = new Buff[]
    {
        Buff.ARMOR,
        Buff.ARMOR,
        Buff.ARMOR,
        Buff.REFLECT,
        Buff.KNIFE,
        Buff.GRAVITY,
        Buff.DODGE,
        Buff.DODGE
    };
    private Buff[] UtilityBuff = new Buff[]
    {
        Buff.BLACK_OUT,
        Buff.SPEED_UP,
        Buff.FLASH,
        Buff.DESTROY,
        Buff.CONSTRUCT,
        Buff.KAMUI,
        Buff.MISSILE,
        Buff.TRAP
    };

    /// <summary>
    /// Initializes the buff selector.
    /// </summary>
    public void BuffInit()
    {
        Random offensiverand = new Random();
        Random defensiverand = new Random();
        Random Utilityrand = new Random();
        OffensiveBuff = OffensiveBuff.OrderBy(x => offensiverand.Next()).ToArray();
        DefensiveBuff = DefensiveBuff.OrderBy(x => defensiverand.Next()).ToArray();
        UtilityBuff = UtilityBuff.OrderBy(x => Utilityrand.Next()).ToArray();

        throw new NotImplementedException();
    }

    /// <summary>
    /// Selects a buff for a player.
    /// </summary>
    /// <param name="player">The player for whom the buff is selected.</param>
    /// <returns>The selected buff.</returns>
    public Buff SelectBuff(Player player)
    {
        // TODO: Implement

        throw new NotImplementedException();
    }
}