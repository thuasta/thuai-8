namespace Thuai.Server.GameLogic;

public partial interface ISkill
{
    public class OnActivationEventArgs(SkillName name) : EventArgs
    {
        public SkillName Name => name;
    }
    public class OnDeactivationEventArgs(SkillName name) : EventArgs
    {
        public SkillName Name => name;
    }

    public event EventHandler<OnActivationEventArgs>? OnActivationEvent;
    public event EventHandler<OnDeactivationEventArgs>? OnDeactivationEvent;
}
