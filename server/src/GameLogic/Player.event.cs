namespace Thuai.Server.GameLogic;

public partial class Player
{
    public enum PlayerEventType
    {
        PlayerMove,
        PlayerAttack,
        PlayerTurn,
    };

    public class PlayerMoveEventArgs : EventArgs
    {
        public const PlayerEventType EventName = PlayerEventType.PlayerMove;
        public Player Player { get; }

        public MoveDirection Movedirection { get; set; }

        public PlayerMoveEventArgs(Player player, MoveDirection direction)
        {
            Player = player;
            Movedirection = direction;
        }
    }
    public class PlayerAttackEventArgs : EventArgs
    {
        public const PlayerEventType EventName = PlayerEventType.PlayerAttack;
        public Player Player { get; }

        public PlayerAttackEventArgs(Player player)
        {
            Player = player;
        }
    }

    public class PlayerTurnEventArgs : EventArgs
    {
        public const PlayerEventType EventName = PlayerEventType.PlayerTurn;
        public Player Player { get; }

        public TurnDirection Turndirection { get; set; }

        public PlayerTurnEventArgs(Player player, TurnDirection direction)
        {
            Player = player;
            Turndirection = direction;
        }
    }
    public event EventHandler<PlayerMoveEventArgs>? PlayerMoveEvent;
    public event EventHandler<PlayerAttackEventArgs>? PlayerAttackEvent;
    public event EventHandler<PlayerTurnEventArgs>? PlayerTurnEvent;
}