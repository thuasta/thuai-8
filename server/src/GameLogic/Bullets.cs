namespace Thuai.Server.GameLogic;


/// <summary>
/// Bullet used by a weapon.
/// </summary>
public interface IBullet
{
    public enum BulletType
    {
        Cannonball,
        FastBullet,
        DamageBullet,
        FinalBullet
    }

    /// <summary>
    /// The type of the bullet.
    /// </summary>
    public BulletType Type { get; }

    public Position BulletPosition { get; set; }
    public double BulletSpeed { get; set; }
    public double BulletDamage { get; set; }

    // TODO: Implement
}

/// <summary>
/// Default bullet for a weapon. Used by Cannon.
/// </summary>
public class Cannonball : IBullet
{
    public IBullet.BulletType Type => IBullet.BulletType.Cannonball;

    public Position BulletPosition { get; set; } = new Position();

    public double BulletSpeed { get; set; }
    public double BulletDamage { get; set; }



}

// TODO: Add more bullets
