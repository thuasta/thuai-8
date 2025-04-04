using nkast.Aether.Physics2D.Dynamics;

namespace Thuai.Server.Physics;

public interface IPhysicalObject
{
    public Body? Body { get; }

    public void Bind(Body body);
    public void Unbind();
}
