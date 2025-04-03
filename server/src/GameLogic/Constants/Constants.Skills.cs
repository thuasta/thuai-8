namespace Thuai.Server.GameLogic;

public static partial class Constants
{
    public static class SkillCooldown
    {
        public const int BLACK_OUT = 600;
        public const int SPEED_UP = 100;
        public const int FLASH = 100;
        public const int DESTROY = 200;
        public const int CONSTRUCT = 100;
        public const int TRAP = 200;
        public const int RECOVER = 600;
        public const int KAMUI = 200;
    }

    public static class SkillDuration
    {
        public const int BLACK_OUT = 10;
        public const int SPEED_UP = 20;
        public const int TRAP = 400;
        public const int KAMUI = 20;
    }

    public static class SkillEffect
    {
        public const float SPPED_UP_FACTOR = 4;
        public const float FLASH_DISTANCE = 10;
        public const int CONSTRUCT_WALL_STRENGTH = 3;
        public const float TRAP_RADIUS = PLAYER_RADIUS;
        public const int TRAP_EFFECT_TICKS = 10;
    }
}
