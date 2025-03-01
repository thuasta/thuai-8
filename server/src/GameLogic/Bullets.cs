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
    public int BulletDamage { get; }

    public bool AntiArmor { get; }

    // TODO: Implement TravelledDistance
}

/// <summary>
/// Default bullet for a weapon. Used by Cannon.
/// </summary>
public class Bullet(Position position, double speed, int damage, bool antiArmor = false) : IBullet
{
    public IBullet.BulletType Type => IBullet.BulletType.Bullet;

    public Position BulletPosition { get; set; } = position;

    public double BulletSpeed { get; } = speed;
    public int BulletDamage { get; } = damage;

    public bool AntiArmor { get; } = antiArmor;
}

public class LaserBullet(Position position, double speed, int damage, bool antiArmor = false) : IBullet
{
    public IBullet.BulletType Type => IBullet.BulletType.Bullet;

    public Position BulletPosition { get; set; } = position;

    public double BulletSpeed { get; } = speed;
    public int BulletDamage { get; } = damage;

    public bool AntiArmor { get; } = antiArmor;
}

// TODO: Add more bullets
