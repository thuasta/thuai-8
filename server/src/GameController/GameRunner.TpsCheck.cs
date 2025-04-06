namespace Thuai.Server.GameController;

public partial class GameRunner
{
    public const double TpsUpperBoundRatio = 1.2;
    public const double TpsLowerBoundRatio = 0.8;
    public const double TpsClockFixRatio = 0.92;
    public const int TpsCheckInterval = 100;
}
