namespace Thuai.Server.Geometry.Shapes;

/// <summary>
/// Represents a shape that can be used for collision detection.
/// </summary>
public interface IShape
{
    public static bool Collides(IShape shape1, IShape shape2)
    {
        throw new NotImplementedException();
    }
}
