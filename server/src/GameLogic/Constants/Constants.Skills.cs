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
        public const int MISSILE = 20;
        public const int KAMUI = 200;
    }

    public static class SkillDuration
    {
        public const int BLACK_OUT = 10;
        public const int SPEED_UP = 20;
        public const int TRAP = 400;
        public const int MISSILE = 40;
        public const int KAMUI = 20;
    }

    public static class SkillEffect
    {
        public const double SPPED_UP_FACTOR = 4;
        public const double FLASH_DISTANCE = 10;
        public const int CONSTRUCT_WALL_STRENGTH = 3;
        public const int TRAP_EFFECT_TICKS = 10;
    }
}
