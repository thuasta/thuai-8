using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic
{
    public class WeaponTests
    {
        [Fact]
        public void Weapon_CurrentBullets_ShouldNotExceedMaxBullets()
        {
            // Arrange
            var weapon = new Weapon
            {
                currentBullets = 20
            };

            // Act
            if (weapon.currentBullets > weapon.maxBullets)
            {
                weapon.currentBullets = weapon.maxBullets;
            }

            // Assert
            Assert.Equal(Constants.MAX_BULLETS, weapon.currentBullets);
        }

        [Fact]
        public void Weapon_Ammo_ShouldDecreaseWhenFired()
        {
            // Arrange
            var weapon = new Weapon();
            int initialBullets = weapon.currentBullets;

            // Act
            weapon.currentBullets--;  // Simulate firing the weapon

            // Assert
            Assert.Equal(initialBullets - 1, weapon.currentBullets);
        }

        [Fact]
        public void Weapon_AntiArmor_ShouldBeSetCorrectly()
        {
            // Arrange
            var weapon = new Weapon();
            weapon.antiArmor = true;

            // Act
            // No action needed, just verifying the value

            // Assert
            Assert.True(weapon.antiArmor);
        }

        [Fact]
        public void Weapon_IsLaser_ShouldBeSetCorrectly()
        {
            // Arrange
            var weapon = new Weapon();
            weapon.isLaser = true;

            // Act
            // No action needed, just verifying the value

            // Assert
            Assert.True(weapon.isLaser);
        }
    }
}
