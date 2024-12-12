namespace Thuai.Server.GameLogic;

/// <summary>
/// Constants used in the game.
/// </summary>
public static class Constants
{
    //Armor
    public const double INITIAL_ARMOR_VALUE = 0;
    public const double INITIAL_HEALTH_VALUE = 1;

    //Weapon
    public const double INITIAL_ATTACK_SPEED = 2;
    public const double INITIAL_BULLET_SPEED = 3;
    public const double FAST_BULLET_SPEED = 5;
    public const int INITIAL_DAMAGE = 1;
    public const double HIGH_DAMAGE = 2;
    public const int MAX_BULLETS = 10;

    //Player
    public const double MOVE_SPEED = 2;
    public const double TURN_SPEED = Math.PI / 18;
    public const double PLAYER_RADIO = 0.1;

    //Map
    public const int WALL_LENGTH = 10;
    public const double WALL_THICK = 0.1;

}
