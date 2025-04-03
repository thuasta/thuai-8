using Thuai.Server.GameLogic;

//Checked original tests 03/17/2025
namespace Thuai.Server.Test.GameLogic
{
    public class ConstantsTests
    {
        [Fact]
        public void Armor_Constants_AreCorrect()
        {
            Assert.Equal(0, Constants.INITIAL_ARMOR_VALUE);
            Assert.Equal(1, Constants.ARMOR_VALUE_INCREASE);
            Assert.Equal(1, Constants.INITIAL_HEALTH_VALUE);
            Assert.Equal(0, Constants.INITIAL_DODGE_PERCENTAGE);
            Assert.Equal(10, Constants.DODGE_PERCENTAGE_INCREASE);
        }

        [Fact]
        public void Weapon_Constants_AreCorrect()
        {
            Assert.Equal(0.1, Constants.INITIAL_ATTACK_SPEED, 1e-5);
            Assert.Equal(0.025, Constants.ATTACK_SPEED_INCREASE, 1e-5);
            Assert.Equal(3, Constants.INITIAL_BULLET_SPEED);
            Assert.Equal(0.5, Constants.BULLET_SPEED_INCREASE, 1e-5);
            Assert.Equal(1, Constants.INITIAL_DAMAGE);
            Assert.Equal(1, Constants.DAMAGE_INCREASE);
            Assert.Equal(10, Constants.INITIAL_BULLETS);
            Assert.Equal(1, Constants.BULLETS_INCREASE);
        }

        // [Fact]
        // public void Skill_Constants_AreCorrect()
        // {
        //     Assert.Equal(200, Constants.SKILL_MAX_COOLDOWN);
        // }

        // [Fact]
        // public void Player_Constants_AreCorrect()
        // {
        //     Assert.Equal(2, Constants.MOVE_SPEED);
        //     Assert.Equal(Math.PI / 18, Constants.TURN_SPEED);
        //     Assert.Equal(0.1, Constants.PLAYER_RADIO);
        // }

        [Fact]
        public void Map_Constants_AreCorrect()
        {
            Assert.Equal(10, Constants.WALL_LENGTH);
            Assert.Equal(0.1, Constants.WALL_THICK);
        }
    }
}
