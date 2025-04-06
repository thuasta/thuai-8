using nkast.Aether.Physics2D.Common;
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

    public static Utility.IdProvider IdProvider { get; } = new();

    /// <summary>
    /// The type of the bullet.
    /// </summary>
    public BulletType Type { get; }

    public int Id { get; }
    public Position BulletPosition { get; }
    public float BulletSpeed { get; }
    public int BulletDamage { get; }

    public bool IsMissile { get; }
    public bool AntiArmor { get; }

    public Weapon Owner { get; }
}

/// <summary>
/// Default bullet for a weapon. Used by Cannon.
/// </summary>
public class Bullet : IBullet, Physics.IPhysicalObject
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

    public int Id { get; }
    public float BulletSpeed { get; }
    public int BulletDamage { get; }
    public bool IsMissile => false;     // TODO: Implement missile
    public bool AntiArmor { get; }
    public bool IsDestroyed => _remainingTicks.IsZero == true || Enabled == false;
    public required Weapon Owner { get; init; }

    public Body? Body { get; private set; }
    public bool Enabled { get; set; } = true;

    private readonly Counter _remainingTicks;

    public Bullet(float speed, int damage, bool antiArmor = false)
    {
        Id = IBullet.IdProvider.GetNextId();
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
        Body.LinearVelocity = new(
                (float)(BulletSpeed * Math.Cos(Body.Rotation)),
                (float)(BulletSpeed * Math.Sin(Body.Rotation))
            );
        Body.Tag = new Physics.Tag() { Owner = this };
        Physics.Tag tag = (Physics.Tag)Body.Tag;
        tag.AttachedData[Physics.Key.CoveredFields] = 0;
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

public class LaserBullet(Position position, float speed, int damage, float length, bool antiArmor = false) : IBullet
{
    public static int CauculateDamage(int initialDamage, float remainingLength, float maximumLength)
    {
        return (int)Math.Ceiling(initialDamage * remainingLength / maximumLength);
    }

    public IBullet.BulletType Type => IBullet.BulletType.LaserBullet;

    public Position BulletPosition { get; } = position;
    public float Length { get; } = length;
    public List<Vector2> Trace { get; set; } = [];

    public int Id => -1;    // TODO: Implement laser bullet ID
    public float BulletSpeed { get; } = speed;
    public int BulletDamage { get; } = damage;

    public bool IsMissile => false; // Laser is not a missile
    public bool AntiArmor { get; } = antiArmor;

    public required Weapon Owner { get; init; }
}
