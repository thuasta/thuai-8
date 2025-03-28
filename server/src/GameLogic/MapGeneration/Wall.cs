namespace Thuai.Server.GameLogic.MapGeneration;

public static class WallDirection
{
    public const int HORIZONTAL = 0;
    public const int VERTICAL = 90;
}

public partial class Wall(int x, int y, int angle, bool breakable = false)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Angle { get; } = angle;
    public bool Breakable { get; } = breakable;
    public int WallDurability { get; private set; } = Constants.SkillEffect.CONSTRUCT_WALL_STRENGTH;
    public bool IsBroken => Breakable == true && WallDurability <= 0;

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

    /// <summary>
    /// Should be called if collide on the wall.
    /// </summary>
    /// <returns>If the wall should disappear.</returns>
    public bool CollideOnWall()
    {
        if (!Breakable)
        {
            return false;
        }
        --WallDurability;
        return WallDurability <= 0;
    }

}
