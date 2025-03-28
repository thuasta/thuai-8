namespace Thuai.Server.GameLogic;

public partial class Player
{
    public bool IsBlinded { get; set; } = false;
    public bool Kamui { get; set; } = false;
    public bool IsInvulnerable => Kamui || PlayerArmor.Knife.IsActivated;
}
