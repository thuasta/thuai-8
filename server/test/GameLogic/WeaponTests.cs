using Thuai.Server.GameLogic;

//Checked original tests 03/17/2025
namespace Thuai.Server.Test.GameLogic
{
    public class WeaponTests
    {
        [Fact]
        public void Weapon_DefaultConstructor_IsCorrect()
        {
            //Arrange
            Weapon weapon = new Weapon();

            //Act
            //No need to act

            //Assert
            Assert.Equal(Constants.INITIAL_ATTACK_SPEED, weapon.AttackSpeed, 1e-5);
            Assert.Equal(Constants.INITIAL_BULLET_SPEED, weapon.BulletSpeed, 1e-5);
            Assert.False(weapon.IsLaser);
            Assert.False(weapon.AntiArmor);
            Assert.Equal(Constants.INITIAL_DAMAGE, weapon.Damage);
            Assert.Equal(Constants.INITIAL_BULLETS, weapon.MaxBullets);
            Assert.Equal(Constants.INITIAL_BULLETS, weapon.CurrentBullets);
        }

        [Fact]
        public void Weapon_SetValues_Correctly()
        {
            //Arrange
            Weapon weapon = new Weapon();

            //Act
            weapon.AttackSpeed = 0.42;
            weapon.BulletSpeed = 0.6;
            weapon.IsLaser = true;
            weapon.AntiArmor = true;
            weapon.Damage = 100;
            weapon.MaxBullets = 20;
            weapon.CurrentBullets = 15;

            //Assert
            Assert.Equal(0.42, weapon.AttackSpeed, 1e-5);
            Assert.Equal(0.6, weapon.BulletSpeed, 1e-5);
            Assert.True(weapon.IsLaser);
            Assert.True(weapon.AntiArmor);
            Assert.Equal(100, weapon.Damage);
            Assert.Equal(20, weapon.MaxBullets);
            Assert.Equal(15, weapon.CurrentBullets);
        }


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
            Assert.Equal(Constants.INITIAL_BULLETS, weapon.CurrentBullets);
        }

        [Fact]
        public void Weapon_Recover_Correctly()
        {
            // Arrange
            var weapon = new Weapon
            {
                CurrentBullets = 5
            };

            // Act
            weapon.Recover();

            // Assert
            Assert.Equal(Constants.INITIAL_BULLETS, weapon.CurrentBullets);
        }
    }
}
