namespace Thuai.GameServer.MapGenerator;

public class Wall(int x, int y, int angle)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Angle { get; } = angle;
    public override bool Equals(object? obj)
    {
        if (obj is Wall other)
        {
            return X == other.X && Y == other.Y && Angle == other.Angle;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Angle);
    }
}