using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic
{
    public class ConstantsTests
    {
        [Fact]
        public void Armor_Constants_AreCorrect()
        {
            Assert.Equal(0, Constants.INITIAL_ARMOR_VALUE);
            Assert.Equal(1, Constants.INITIAL_HEALTH_VALUE);
        }

        [Fact]
        public void Weapon_Constants_AreCorrect()
        {
            Assert.Equal(2, Constants.INITIAL_ATTACK_SPEED);
            Assert.Equal(3, Constants.INITIAL_BULLET_SPEED);
            Assert.Equal(5, Constants.FAST_BULLET_SPEED);
            Assert.Equal(1, Constants.INITIAL_DAMAGE);
            Assert.Equal(2, Constants.HIGH_DAMAGE);
            Assert.Equal(10, Constants.MAX_BULLETS);
        }

        [Fact]
        public void Player_Constants_AreCorrect()
        {
            Assert.Equal(2, Constants.MOVE_SPEED);
            Assert.Equal(Math.PI / 18, Constants.TURN_SPEED);
            Assert.Equal(0.1, Constants.PLAYER_RADIO);
        }

        [Fact]
        public void Map_Constants_AreCorrect()
        {
            Assert.Equal(10, Constants.WALL_LENGTH);
            Assert.Equal(0.1, Constants.WALL_THICK);
        }
    }
}
