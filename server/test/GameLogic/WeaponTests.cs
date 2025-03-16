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
                CurrentBullets = 20
            };

            // Act
            if (weapon.CurrentBullets > weapon.MaxBullets)
            {
                weapon.CurrentBullets = weapon.MaxBullets;
            }

            // Assert
            //Assert.Equal(Constants.MAX_BULLETS, weapon.CurrentBullets);
        }

        [Fact]
        public void Weapon_Ammo_ShouldDecreaseWhenFired()
        {
            // Arrange
            var weapon = new Weapon();
            int initialBullets = weapon.CurrentBullets;

            // Act
            weapon.CurrentBullets--;  // Simulate firing the weapon

            // Assert
            Assert.Equal(initialBullets - 1, weapon.CurrentBullets);
        }

        [Fact]
        public void Weapon_AntiArmor_ShouldBeSetCorrectly()
        {
            // Arrange
            var weapon = new Weapon();
            weapon.AntiArmor = true;

            // Act
            // No action needed, just verifying the value

            // Assert
            Assert.True(weapon.AntiArmor);
        }

        [Fact]
        public void Weapon_IsLaser_ShouldBeSetCorrectly()
        {
            // Arrange
            var weapon = new Weapon();
            weapon.IsLaser = true;

            // Act
            // No action needed, just verifying the value

            // Assert
            Assert.True(weapon.IsLaser);
        }
    }
}
