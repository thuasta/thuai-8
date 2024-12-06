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

    public Position BulletPosition { get; set; }
    public double BulletSpeed { get; }
    public double BulletDamage { get; }

    public bool AntiArmor { get; }

    // TODO: Implement
}

/// <summary>
/// Default bullet for a weapon. Used by Cannon.
/// </summary>
public class Bullet : IBullet
{
    public IBullet.BulletType Type => IBullet.BulletType.Bullet;

    public Position BulletPosition { get; set; }

    public double BulletSpeed { get; }
    public double BulletDamage { get; }

    public bool AntiArmor { get; }

    public Bullet(Position position, double speed, double damage, bool antiArmor = false)
    {
        BulletPosition = position;
        BulletSpeed = speed;
        BulletDamage = damage;
        AntiArmor = antiArmor;
    }
}

public class LaserBullet : IBullet
{
    public IBullet.BulletType Type => IBullet.BulletType.Bullet;

    public Position BulletPosition { get; set; }

    public double BulletSpeed { get; }
    public double BulletDamage { get; }

    public bool AntiArmor { get; }

    public LaserBullet(Position position, double speed, double damage, bool antiArmor = false)
    {
        BulletPosition = position;
        BulletSpeed = speed;
        BulletDamage = damage;
        AntiArmor = antiArmor;
    }
}

// TODO: Add more bullets
