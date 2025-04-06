using nkast.Aether.Physics2D.Dynamics;

namespace Thuai.Server.GameLogic;

public class Trap : Physics.IPhysicalObject
{
    public Body? Body { get; private set; } = null;
    public bool Enabled { get; set; } = true;

    public bool IsDestroyed => _remainingTicks.IsZero == true || Enabled == false;
    public required Player Owner { get; init; }
    public Position TrapPosition
    {
        get
        {
            if (Body is null)
            {
                throw new InvalidOperationException("Trap is not bound to a body.");
            }
            return new(Body.Position.X, Body.Position.Y, Body.Rotation);
        }
    }

    private readonly Counter _remainingTicks;

    public Trap()
    {
        _remainingTicks = new(Constants.SkillDuration.TRAP);
        _remainingTicks.Reset();
    }

    public void Update()
    {
        _remainingTicks.Decrease();
    }

    public void Bind(Body body)
    {
        Body = body;
        Body.Tag = new Physics.Tag() { Owner = this };
    }
    public void Unbind()
    {
        if (Body is null)
        {
            return;
        }
        Body.Tag = new();
        Body = null;
    }
}
