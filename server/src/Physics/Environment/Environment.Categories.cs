using nkast.Aether.Physics2D.Dynamics;

namespace Thuai.Server.Physics;

public partial class Environment
{
    public static class Categories
    {
        public const Category Player = Category.Cat1;
        public const Category Wall = Category.Cat2;
        public const Category Bullet = Category.Cat3;
        public const Category Laser = Category.Cat4;
        public const Category Trap = Category.Cat5;
        public const Category GravityField = Category.Cat6;
        public const Category Grid = Category.Cat7;
    }

    public static class CollisionList
    {
        public const Category PlayerCollidesWith =
            Categories.Wall | Categories.Bullet | Categories.Laser | Categories.Trap | Categories.GravityField;
        public const Category WallCollidesWith =
            Categories.Player | Categories.Bullet;
        public const Category BulletCollidesWith =
            Categories.Player | Categories.Wall | Categories.GravityField;
        public const Category LaserCollidesWith =
            Categories.Player;
        public const Category TrapCollidesWith =
            Categories.Player;
        public const Category GravityFieldCollidesWith =
            Categories.Player | Categories.Bullet;
        public const Category GridCollidesWith = Category.None;
    }
}
