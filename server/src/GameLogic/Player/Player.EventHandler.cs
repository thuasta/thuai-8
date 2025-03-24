namespace Thuai.Server.GameLogic;

public partial class Player
{
    public void OnSkillActivation(object? sender, ISkill.OnActivationEventArgs e)
    {
        switch (e.Name)
        {
            case SkillName.BLACK_OUT:
                SkillActivationEvent?.Invoke(this, new(this, e.Name));
                break;

            default:
                throw new ArgumentException($"Invalid skill name {e.Name}.");
        }
    }

    public void OnSkillDeactivation(object? sender, ISkill.OnDeactivationEventArgs e)
    {
        switch (e.Name)
        {
            case SkillName.BLACK_OUT:
                SkillDeactivationEvent?.Invoke(this, new(this, e.Name));
                break;

            default:
                throw new ArgumentException($"Invalid skill name {e.Name}.");
        }
    }
}
