using Serilog.Debugging;
using Thuai.Server.GameController;

namespace Thuai.Server.Buff;

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
/// Selects a buff for a player.
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
    /// <param name="player","num">The player and the number of the buff.</param>
    /// <returns> void </returns>
    public void SelectBuff(Player player, int num)
    {
        switch (num)
        {
            case 1:
                ChooseOffensiveBuff(player, OffensiveBuff[Round - 1]);
                break;
            case 2:
                ChooseDefensiveBuff(player, DefensiveBuff[Round - 1]);
                break;
            case 3:
                ChooseUtilityBuff(player, UtilityBuff[Round - 1]);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chooses an offensive buff.
    /// </summary>
    /// <param name="player","buff">The player and the buff.</param>
    /// <returns> void </returns>
    private void ChooseOffensiveBuff(Player player, Buff buff)
    {
        switch (buff)
        {
            case Buff.BULLET_COUNT:
                //to be continued
                break;
            case Buff.BULLET_SPEED:
                //to be continued
                break;
            case Buff.ATTACK_SPEED:
                //to be continued
                break;
            case Buff.LASER:
                //to be continued
                break;
            case Buff.DAMAGE:
                //to be continued
                break;
            case Buff.ANTI_ARMOR:
                //to be continued
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chooses a defensive buff.
    /// </summary>
    /// <param name="player","buff">The player and the buff.</param>
    /// <returns> void </returns>
    private void ChooseDefensiveBuff(Player player, Buff buff)
    {
        switch (buff)
        {
            case Buff.ARMOR:
                //to be continued
                break;
            case Buff.REFLECT:
                //to be continued
                break;
            case Buff.DODGE:
                //to be continued
                break;
            case Buff.KNIFE:
                //to be continued
                break;
            case Buff.GRAVITY:
                //to be continued
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chooses a utility buff.
    /// </summary>
    /// <param name="player","buff">The player and the buff.</param>
    /// <returns> void </returns>
    private void ChooseUtilityBuff(Player player, Buff buff)
    {
        switch (buff)
        {
            case Buff.BLACK_OUT:
                //to be continued
                break;
            case Buff.SPEED_UP:
                //to be continued
                break;
            case Buff.FLASH:
                //to be continued
                break;
            case Buff.DESTROY:
                //to be continued
                break;
            case Buff.CONSTRUCT:
                //to be continued
                break;
            case Buff.TRAP:
                //to be continued
                break;
            case Buff.MISSILE:
                //to be continued
                break;
            case Buff.KAMUI:
                //to be continued
                break;
            default:
                break;
        }
    }

}