namespace Thuai.GameServer.MapGenerator;

class Wall 
{
    public Wall(int x, int y, int angle)
    {
        X = x;
        Y = y;
        Angle = angle;
    }
    public int X { get; }
    public int Y { get; }
    public int Angle { get; }
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