using nkast.Aether.Physics2D.Dynamics;

namespace Thuai.Server.GameLogic;

/// <summary>
/// Bullet used by a weapon.
/// </summary>
public interface IBullet
{
    public enum BulletType
    {
        Bullet,
        LaserBullet
    }

    /// <summary>
    /// The type of the bullet.
    /// </summary>
    public BulletType Type { get; }

    public Position BulletPosition { get; }
    public float BulletSpeed { get; }
    public int BulletDamage { get; }

    public bool AntiArmor { get; }
}

/// <summary>
/// Default bullet for a weapon. Used by Cannon.
/// </summary>
public class Bullet: IBullet, Physics.IPhysicalObject
{
    public IBullet.BulletType Type => IBullet.BulletType.Bullet;

    public Position BulletPosition
    {
        get
        {
            if (Body is null)
            {
                throw new InvalidOperationException("Bullet is not bound to a body.");
            }
            return new(Body.Position.X, Body.Position.Y, Body.Rotation);
        }
    }

    public float BulletSpeed { get; }
    public int BulletDamage { get; }
    public bool AntiArmor { get; }
    public bool IsDestroyed => _remainingTicks.IsZero == true || Body?.Enabled == false;

    public Body? Body { get; private set; }

    private readonly Counter _remainingTicks;

    public Bullet(float speed, int damage, bool antiArmor = false)
    {
        BulletSpeed = speed;
        BulletDamage = damage;
        AntiArmor = antiArmor;
        _remainingTicks = new(Constants.BULLET_REMAINING_TICKS);
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

public class LaserBullet(Position position, float speed, int damage, bool antiArmor = false) : IBullet
{
    public IBullet.BulletType Type => IBullet.BulletType.LaserBullet;

    public Position BulletPosition { get; } = position;

    public float BulletSpeed { get; } = speed;
    public int BulletDamage { get; } = damage;

    public bool AntiArmor { get; } = antiArmor;
}
