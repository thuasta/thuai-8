namespace Thuai.Server.GameLogic.Buff;

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
    RECOVER,
    KAMUI, // 虚化
}

/// <summary>
/// Selects a buff for a player.
/// </summary>
public class BuffSelector
{
    public const int BUFF_KINDS = 3;

    /// <summary>
    /// Three types of buffs.
    /// </summary>
    private Buff[] OffensiveBuff =
    [
        Buff.BULLET_COUNT,
        Buff.BULLET_COUNT,
        Buff.BULLET_SPEED,
        Buff.BULLET_SPEED,
        Buff.ATTACK_SPEED,
        Buff.DAMAGE,
        Buff.LASER,
        Buff.ANTI_ARMOR
    ];
    private Buff[] DefensiveBuff =
    [
        Buff.ARMOR,
        Buff.ARMOR,
        Buff.ARMOR,
        Buff.REFLECT,
        Buff.KNIFE,
        Buff.GRAVITY,
        Buff.DODGE,
        Buff.DODGE
    ];
    private Buff[] UtilityBuff =
    [
        Buff.BLACK_OUT,
        Buff.SPEED_UP,
        Buff.FLASH,
        Buff.DESTROY,
        Buff.CONSTRUCT,
        Buff.KAMUI,
        Buff.RECOVER,
        Buff.TRAP
    ];

    private int _round = 1;

    private readonly Random _random = new();

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
        OffensiveBuff = [.. OffensiveBuff.OrderBy(x => _random.Next())];
        DefensiveBuff = [.. DefensiveBuff.OrderBy(x => _random.Next())];
        UtilityBuff = [.. UtilityBuff.OrderBy(x => _random.Next())];
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
        _round = round;
        availableBuff[0] = OffensiveBuff[round - 1];
        availableBuff[1] = DefensiveBuff[round - 1];
        availableBuff[2] = UtilityBuff[round - 1];
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
                ChooseOffensiveBuff(player, OffensiveBuff[_round - 1]);
                break;
            case 2:
                ChooseDefensiveBuff(player, DefensiveBuff[_round - 1]);
                break;
            case 3:
                ChooseUtilityBuff(player, UtilityBuff[_round - 1]);
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
                O_Buff.BULLET_COUNT(player);
                break;
            case Buff.BULLET_SPEED:
                O_Buff.BULLET_SPEED(player);
                break;
            case Buff.ATTACK_SPEED:
                O_Buff.ATTACK_SPEED(player);
                break;
            case Buff.LASER:
                O_Buff.LASER(player);
                break;
            case Buff.DAMAGE:
                O_Buff.DAMAGE(player);
                break;
            case Buff.ANTI_ARMOR:
                O_Buff.ANTI_ARMOR(player);
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
                D_Buff.ARMOR(player);
                break;
            case Buff.REFLECT:
                D_Buff.REFLECT(player);
                break;
            case Buff.DODGE:
                D_Buff.DODGE(player);
                break;
            case Buff.KNIFE:
                D_Buff.KNIFE(player);
                break;
            case Buff.GRAVITY:
                D_Buff.GRAVITY(player);
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
                U_Buff.BLACK_OUT(player);
                break;
            case Buff.SPEED_UP:
                U_Buff.SPEED_UP(player);
                break;
            case Buff.FLASH:
                U_Buff.FLASH(player);
                break;
            case Buff.DESTROY:
                U_Buff.DESTROY(player);
                break;
            case Buff.CONSTRUCT:
                U_Buff.CONSTRUCT(player);
                break;
            case Buff.TRAP:
                U_Buff.TRAP(player);
                break;
            case Buff.RECOVER:
                U_Buff.RECOVER(player);
                break;
            case Buff.KAMUI:
                U_Buff.KAMUI(player);
                break;
            default:
                break;
        }
    }
}