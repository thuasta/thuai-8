namespace Thuai.Server.GameLogic;

/// <summary>
/// Bullet used by a weapon.
/// </summary>
public interface IBullet
{
    public enum BulletType
    {
        Cannonball,
    }

    /// <summary>
    /// The type of the bullet.
    /// </summary>
    public BulletType Type { get; }

    // TODO: Implement
}

/// <summary>
/// Default bullet for a weapon. Used by Cannon.
/// </summary>
public class Cannonball : IBullet
{
    public IBullet.BulletType Type => IBullet.BulletType.Cannonball;

    // TODO: Implement
}

// TODO: Add more bullets
