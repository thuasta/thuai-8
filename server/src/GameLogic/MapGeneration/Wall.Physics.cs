using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

namespace Thuai.Server.GameLogic.MapGeneration;

public partial class Wall : Physics.IPhysicalObject
{
    public Body? Body { get; private set; }
    public bool Enabled { get; set; } = true;

    public void Bind(Body body)
    {
        Body = body;
        Body.Tag = new Physics.Tag() { Owner = this };

        Physics.Tag tag = (Physics.Tag)Body.Tag;
        tag.AttachedData[Physics.Key.CorrespondingWallPosition] = new Protocol.Scheme.PositionInt()
        {
            X = X,
            Y = Y,
            Angle = Angle,
        };

        Body.OnCollision += OnCollision;
    }

    public void Unbind()
    {
        if (Body is null)
        {
            return;
        }

        Body.OnCollision -= OnCollision;
        Body.Tag = new();
        Body = null;
    }

    public bool OnCollision(Fixture a, Fixture b, Contact contact)
    {
        if (b.CollisionCategories == Physics.Environment.Categories.Bullet)
        {
            CollideOnWall();
        }
        return true;
    }
}
