namespace Thuai.Server.GameLogic;

public class Position
{
    public double Xpos { get; set; }
    public double Ypos { get; set; }

    public double Angle { get; set; }
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