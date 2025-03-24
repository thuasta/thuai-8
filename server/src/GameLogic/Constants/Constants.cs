namespace Thuai.Server.GameLogic;

/// <summary>
/// Constants used in the game.
/// </summary>
public static partial class Constants
{
    //Armor
    public const int INITIAL_ARMOR_VALUE = 0;
    public const int ARMOR_VALUE_INCREASE = 1;
    public const int INITIAL_HEALTH_VALUE = 2;
    public const int INITIAL_DODGE_PERCENTAGE = 0;      // 0%
    public const int DODGE_PERCENTAGE_INCREASE = 10;    // 10%
    public const double GRAVITY_FIELD_RADIUS = 1.5;
    public const double GRAVITY_FIELD_STRENGTH = 0.5;
    public const int KNIFE_REMAINING_TIME = 10;

    //Weapon
    public const double INITIAL_ATTACK_SPEED = 0.1;
    public const double ATTACK_SPEED_INCREASE = 0.1;
    public const double INITIAL_BULLET_SPEED = 3;
    public const double BULLET_SPEED_INCREASE = 0.5;
    public const int INITIAL_DAMAGE = 1;
    public const int DAMAGE_INCREASE = 1;
    public const int INITIAL_BULLETS = 10;
    public const int BULLETS_INCREASE = 1;
    public const int ANTI_ARMOR_FACTOR = 2;

    //Player
    public const double MOVE_SPEED = 0.1;
    public const double TURN_SPEED = Math.PI / 60;
    public const double PLAYER_RADIUS = 0.3;
    public const double BULLET_GENERATE_DISTANCE = 0.5;

    // Bullet
    public const double BULLET_RADIUS = 0.1;

    //Map
    public const double WALL_LENGTH = 1.2;
    public const double WALL_THICK = 0.1;

    // Wall
    public const int BREAKABLE_WALL_MAX_COLLIDE_COUNT = 3;
}
