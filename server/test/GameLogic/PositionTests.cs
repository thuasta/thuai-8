using Thuai.Server.GameLogic;

//Checked original tests 03/17/2025
namespace Thuai.Server.Test.GameLogic
{
    public class PositionTests
    {
        [Fact]
        public void Position_Constructor_ShouldSetValuesCorrectly()
        {
            // Arrange
            double expectedX = 5.0;
            double expectedY = 10.0;
            double expectedAngle = Math.PI / 2;  // 90 degrees in radians

            // Act
            var position = new Position(expectedX, expectedY, expectedAngle);

            // Assert
            Assert.Equal(expectedX, position.Xpos, 1e-5);
            Assert.Equal(expectedY, position.Ypos, 1e-5);
            Assert.Equal(expectedAngle, position.Angle, 1e-5);
        }

        [Fact]
        public void Position_DefaultConstructor_ShouldSetDefaultValues()
        {
            // Act
            var position = new Position();

            // Assert
            Assert.Equal(0, position.Xpos);
            Assert.Equal(0, position.Ypos);
            Assert.Equal(0, position.Angle);
        }

        [Fact]
        public void MoveDirection_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)MoveDirection.NONE);
            Assert.Equal(1, (int)MoveDirection.BACK);
            Assert.Equal(2, (int)MoveDirection.FORTH);
        }

        [Fact]
        public void TurnDirection_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)TurnDirection.NONE);
            Assert.Equal(1, (int)TurnDirection.CLOCKWISE);
            Assert.Equal(2, (int)TurnDirection.COUNTER_CLOCKWISE);
        }
    }
}
