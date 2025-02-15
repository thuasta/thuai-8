using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic
{
    public class BulletTests
    {
        [Fact]
        public void Bullet_DefaultValues_AreCorrect()
        {
            // Arrange.
            var position = new Position(0, 0); // Assuming a Position class constructor that takes X and Y.
            var bullet = new Bullet(position, 10.0, 5.0, true);

            // Act.
            // No need to act as we are testing default constructor values.

            // Assert.
            Assert.Equal(IBullet.BulletType.Bullet, bullet.Type);
            Assert.Equal(position, bullet.BulletPosition);
            Assert.Equal(10.0, bullet.BulletSpeed);
            Assert.Equal(5.0, bullet.BulletDamage);
            Assert.True(bullet.AntiArmor);
        }

        [Fact]
        public void LaserBullet_DefaultValues_AreCorrect()
        {
            // Arrange.
            var position = new Position(1, 1); // Another position for LaserBullet.
            var laserBullet = new LaserBullet(position, 15.0, 10.0, false);

            // Act.
            // No need to act as we are testing default constructor values.

            // Assert.
            Assert.Equal(IBullet.BulletType.Bullet, laserBullet.Type);
            Assert.Equal(position, laserBullet.BulletPosition);
            Assert.Equal(15.0, laserBullet.BulletSpeed);
            Assert.Equal(10.0, laserBullet.BulletDamage);
            Assert.False(laserBullet.AntiArmor);
        }

        [Fact]
        public void Bullet_AntiArmor_FalseByDefault()
        {
            // Arrange.
            var position = new Position(2, 2);
            var bullet = new Bullet(position, 20.0, 15.0);

            // Act.
            // No additional action needed, this test focuses on default value for AntiArmor.

            // Assert.
            Assert.False(bullet.AntiArmor);
        }
    }
}
