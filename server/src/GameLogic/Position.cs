namespace Thuai.Server.GameLogic;

public class Position
{
    public double Xpos { get; set; }
    public double Ypos { get; set; }

    public double Angle { get; set; } // value are radians: 0 - 2 * Math.PI

    public Position(double x = 0, double y = 0, double angle = 0)
    {
        Xpos = x;
        Ypos = y;
        Angle = angle;
    }
}

public enum MoveDirection
{
    BACK,
    FORTH,
};

public enum TurnDirection
{
    CLOCKWISE,
    COUNTER_CLOCKWISE,
};