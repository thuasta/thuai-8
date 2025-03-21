namespace Thuai.Server.GameLogic;

public partial class Player
{
    public enum PlayerEventType
    {
        PlayerMove,
        PlayerAttack,
        PlayerTurn,
        PlayerPerformSkill
    };

    public class PlayerMoveEventArgs(Player player, MoveDirection direction) : EventArgs
    {
        public const PlayerEventType EventName = PlayerEventType.PlayerMove;
        public Player Player { get; } = player;

        public MoveDirection Movedirection { get; set; } = direction;
    }
    public class PlayerAttackEventArgs(Player player) : EventArgs
    {
        public const PlayerEventType EventName = PlayerEventType.PlayerAttack;
        public Player Player { get; } = player;
    }

    public class PlayerTurnEventArgs(Player player, TurnDirection direction) : EventArgs
    {
        public const PlayerEventType EventName = PlayerEventType.PlayerTurn;
        public Player Player { get; } = player;

        public TurnDirection Turndirection { get; set; } = direction;
    }

    public class PlayerPerformSkillEventArgs(Player player, SkillName skillName) : EventArgs
    {
        public const PlayerEventType EventName = PlayerEventType.PlayerPerformSkill;
        public Player Player = player;
        public SkillName SkillName = skillName;
    }

    public event EventHandler<PlayerMoveEventArgs>? PlayerMoveEvent;
    public event EventHandler<PlayerAttackEventArgs>? PlayerAttackEvent;
    public event EventHandler<PlayerTurnEventArgs>? PlayerTurnEvent;

    public event EventHandler<PlayerPerformSkillEventArgs>? PlayerPerformSkillEvent;
}