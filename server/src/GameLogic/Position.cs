namespace Thuai.Server.GameLogic;

public class Position(double x = 0, double y = 0, double angle = 0)
{
    public double Xpos { get; set; } = x;
    public double Ypos { get; set; } = y;

    public double Angle { get; set; } = angle;
}

public enum MoveDirection
{
    NONE,
    BACK,
    FORTH
};

public enum TurnDirection
{
    NONE,
    CLOCKWISE,
    COUNTER_CLOCKWISE,
};