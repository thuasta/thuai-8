namespace Thuai.Server.GameLogic;

/// <summary>
/// Represents a weapon that can be used by a character.
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// Default weapon for a character.
    /// </summary>
    public static IWeapon DefaultWeapon => new Cannon();

    /// <summary>
    /// The type of bullet that the weapon shoots.
    /// </summary>
    public IBullet.BulletType BulletType { get; }

    // TODO: Implement
}

/// <summary>
/// Default weapon for a character. Shoots cannonballs.
/// </summary>
public class Cannon : IWeapon
{
    public IBullet.BulletType BulletType => IBullet.BulletType.Cannonball;

    // TODO: Implement
}

// TODO: Add more weapons
