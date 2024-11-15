using Serilog.Debugging;
using Thuai.Server.GameLogic;
using Thuai.Server.Buff;

namespace Thuai.Server.BuffSelector;


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
/// Selects a buff for a tank.
/// </summary>
public class BuffSelector
{
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
    private int Round = 1;

    /// <summary>
    /// Contructor.
    /// </summary>
    public BuffSelector()
    {
        BuffInit();
    }

    /// <summary>
    /// Initializes the buff selector.
    /// </summary>
    public bool BuffInit()
    {
        Random offensiverand = new Random();
        Random defensiverand = new Random();
        Random Utilityrand = new Random();
        OffensiveBuff = OffensiveBuff.OrderBy(x => offensiverand.Next()).ToArray();
        DefensiveBuff = DefensiveBuff.OrderBy(x => defensiverand.Next()).ToArray();
        UtilityBuff = UtilityBuff.OrderBy(x => Utilityrand.Next()).ToArray();
        return true;
    }

    /// <summary>
    /// Show the available buffs.
    /// </summary>
    /// <param name="round">The round number.</param>
    /// <returns>The available buffs.</returns>
    public Buff[] ShowBuff(int round)
    {
        Buff[] availableBuff = new Buff[3];
        Round = round;
        availableBuff[0] = OffensiveBuff[round-1];
        availableBuff[1] = DefensiveBuff[round-1];
        availableBuff[2] = UtilityBuff[round-1];
        return availableBuff;
    }

    /// <summary>
    /// Selects a buff.
    /// </summary>
    /// <param name="tank","num">The tank and the number of the buff.</param>
    /// <returns> void </returns>
    public void SelectBuff(Tank tank, int num)
    {
        switch (num)
        {
            case 1:
                ChooseOffensiveBuff(tank, OffensiveBuff[Round - 1]);
                break;
            case 2:
                ChooseDefensiveBuff(tank, DefensiveBuff[Round - 1]);
                break;
            case 3:
                ChooseUtilityBuff(tank, UtilityBuff[Round - 1]);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chooses an offensive buff.
    /// </summary>
    /// <param name="tank","buff">The tank and the buff.</param>
    /// <returns> void </returns>
    private void ChooseOffensiveBuff(Tank tank, Buff buff)
    {
        switch (buff)
        {
            case Buff.BULLET_COUNT:
                O_Buff.BULLET_COUNT(tank);
                break;
            case Buff.BULLET_SPEED:
                O_Buff.BULLET_SPEED(tank);
                break;
            case Buff.ATTACK_SPEED:
                O_Buff.ATTACK_SPEED(tank);
                break;
            case Buff.LASER:
                O_Buff.LASER(tank);
                break;
            case Buff.DAMAGE:
                O_Buff.DAMAGE(tank);
                break;
            case Buff.ANTI_ARMOR:
                O_Buff.ANTI_ARMOR(tank);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chooses a defensive buff.
    /// </summary>
    /// <param name="tank","buff">The tank and the buff.</param>
    /// <returns> void </returns>
    private void ChooseDefensiveBuff(Tank tank, Buff buff)
    {
        switch (buff)
        {
            case Buff.ARMOR:
                D_Buff.ARMOR(tank);
                break;
            case Buff.REFLECT:
                D_Buff.REFLECT(tank);
                break;
            case Buff.DODGE:
                D_Buff.DODGE(tank);
                break;
            case Buff.KNIFE:
                D_Buff.KNIFE(tank);
                break;
            case Buff.GRAVITY:
                D_Buff.GRAVITY(tank);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chooses a utility buff.
    /// </summary>
    /// <param name="tank","buff">The tank and the buff.</param>
    /// <returns> void </returns>
    private void ChooseUtilityBuff(Tank tank, Buff buff)
    {
        switch (buff)
        {
            case Buff.BLACK_OUT:
                U_Buff.BLACK_OUT(tank);
                break;
            case Buff.SPEED_UP:
                U_Buff.SPEED_UP(tank);
                break;
            case Buff.FLASH:
                U_Buff.FLASH(tank);
                break;
            case Buff.DESTROY:
                U_Buff.DESTROY(tank);
                break;
            case Buff.CONSTRUCT:
                U_Buff.CONSTRUCT(tank);
                break;
            case Buff.TRAP:
                U_Buff.TRAP(tank);
                break;
            case Buff.MISSILE:
                U_Buff.MISSILE(tank);
                break;
            case Buff.KAMUI:
                U_Buff.KAMUI(tank);
                break;
            default:
                break;
        }
    }
}