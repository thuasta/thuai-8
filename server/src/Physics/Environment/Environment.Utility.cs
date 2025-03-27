using nkast.Aether.Physics2D.Common;

namespace Thuai.Server.Physics;

public partial class Environment
{
    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.X * b.X + a.Y * b.Y;
    }

    public static Vector2 Reflect(Vector2 vector, Vector2 normal)
    {
        return vector - 2 * Dot(vector, normal) * normal;
    }
}
