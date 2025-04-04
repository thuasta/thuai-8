using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;

namespace Thuai.Server.Physics;

public interface IPhysicalObject
{
    public Body? Body { get; }

    public void Bind(Body body);
    public void Unbind();
}
