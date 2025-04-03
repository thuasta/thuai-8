namespace Thuai.Server.GameLogic;

public class Position(float x = 0, float y = 0, float angle = 0)
{
    public float Xpos { get; set; } = x;
    public float Ypos { get; set; } = y;

    public float Angle { get; set; } = angle;   // Angle in radians
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