namespace Thuai.Server.GameLogic;

public partial class Player
{
    public bool IsBlinded { get; set; } = false;
    public bool Kamui { get; private set; } = false;
    public bool IsStunned => _stunCounter.IsZero == false && IsInvulnerable == false;
    public bool IsInvulnerable => Kamui || PlayerArmor.Knife.IsActivated;

    private readonly Counter _stunCounter = new(Constants.SkillEffect.TRAP_EFFECT_TICKS);
}
