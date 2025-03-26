using Thuai.Server.GameLogic.MapGenerator;
using Xunit;

//Checked original tests 03/17/2025(although the tests are weak)
namespace Thuai.Server.Test.GameLogic
{
    public class WallTests
    {
        [Fact]
        public void Wall_Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            int x = 5;
            int y = 10;
            int angle = 90;

            // Act
            var wall = new Wall(x, y, angle);

            // Assert
            Assert.Equal(x, wall.X);
            Assert.Equal(y, wall.Y);
            Assert.Equal(angle, wall.Angle);
        }

        [Fact]
        public void Wall_Equals_ShouldReturnTrueForEqualWalls()
        {
            // Arrange
            var wall1 = new Wall(5, 10, 90);
            var wall2 = new Wall(5, 10, 90);

            // Act
            var result = wall1.Equals(wall2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Wall_Equals_ShouldReturnFalseForDifferentWalls()
        {
            // Arrange
            var wall1 = new Wall(5, 10, 90);
            var wall2 = new Wall(5, 10, 0); // Different angle

            // Act
            var result = wall1.Equals(wall2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Wall_GetHashCode_ShouldReturnSameHashForEqualWalls()
        {
            // Arrange
            var wall1 = new Wall(5, 10, 90);
            var wall2 = new Wall(5, 10, 90);

            // Act
            var hash1 = wall1.GetHashCode();
            var hash2 = wall2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void Wall_GetHashCode_ShouldReturnDifferentHashForDifferentWalls()
        {
            // Arrange
            var wall1 = new Wall(5, 10, 90);
            var wall2 = new Wall(5, 10, 0); // Different angle

            // Act
            var hash1 = wall1.GetHashCode();
            var hash2 = wall2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
