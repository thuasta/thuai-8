namespace Thuai.Server.GameLogic;

public partial class Player
{

    public class PlayerMoveEventArgs(Player player, MoveDirection direction) : EventArgs
    {
        public Player Player { get; } = player;
        public MoveDirection Movedirection { get; set; } = direction;
    }
    public class PlayerAttackEventArgs(Player player) : EventArgs
    {
        public Player Player { get; } = player;
    }
    public class PlayerTurnEventArgs(Player player, TurnDirection direction) : EventArgs
    {
        public Player Player { get; } = player;
        public TurnDirection Turndirection { get; set; } = direction;
    }
    public class SkillActivationEventArgs(Player player, SkillName skillName) : EventArgs
    {
        public Player Player = player;
        public SkillName SkillName = skillName;
    }
    public class SkillDeactivationEventArgs(Player player, SkillName skillName) : EventArgs
    {
        public Player Player = player;
        public SkillName SkillName = skillName;
    }

    public event EventHandler<PlayerMoveEventArgs>? PlayerMoveEvent = delegate { };
    public event EventHandler<PlayerAttackEventArgs>? PlayerAttackEvent = delegate { };
    public event EventHandler<PlayerTurnEventArgs>? PlayerTurnEvent = delegate { };
    public event EventHandler<SkillActivationEventArgs>? SkillActivationEvent = delegate { };
    public event EventHandler<SkillDeactivationEventArgs>? SkillDeactivationEvent = delegate { };
}
