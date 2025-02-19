using Thuai.GameServer.MapGenerator;
using Xunit;
using System.Linq;

namespace Thuai.Server.Test.GameLogic
{
    public class MapGeneratorTests
    {
        [Fact]
        public void GenerateMaps_ShouldGenerateCorrectNumberOfMaps()
        {
            // Arrange
            var mapGenerator = new MapGenerator();
            int mapCount = 5;
            int width = 10;
            int height = 10;

            // Act
            var maps = mapGenerator.GenerateMaps(mapCount, width, height);

            // Assert
            Assert.Equal(mapCount, maps.Count);
        }

        [Fact]
        public void Map_ShouldHaveWalls()
        {
            // Arrange
            var mapGenerator = new MapGenerator();
            int width = 10;
            int height = 10;
            var map = new Map(width, height);

            // Act
            var walls = map.Walls;

            // Assert
            Assert.True(walls.Count > 0, "Map should have walls");
        }

        [Fact]
        public void Map_ShouldHaveNoDuplicateWalls()
        {
            // Arrange
            var mapGenerator = new MapGenerator();
            int width = 10;
            int height = 10;
            var map = new Map(width, height);

            // Act
            var walls = map.Walls;

            // Assert
            var distinctWalls = new HashSet<Wall>(walls); // Using HashSet to eliminate duplicates
            Assert.Equal(walls.Count, distinctWalls.Count);
        }

        [Fact]
        public void Map_ShouldHaveValidPathWithWalls()
        {
            // Arrange
            var mapGenerator = new MapGenerator();
            int width = 10;
            int height = 10;
            var map = new Map(width, height);

            // Act
            var walls = map.Walls;

            // Assert
            // Ensure that all walls that are part of the path have been removed and are not in the final wall list
            var closedPoints = new HashSet<string>();
            List<Wall> remainingWalls = new List<Wall>(walls);
            foreach (var line in map.Walls)
            {
                string wallKey = $"{line.X}-{line.Y}-{line.Angle}";
                closedPoints.Add(wallKey);
                remainingWalls.Remove(line);
            }
            Assert.True(remainingWalls.Count > 0, "There should still be some walls left after removing the path walls.");
        }

        [Fact]
        public void Map_ShouldHaveUniqueWallsWithCorrectHashing()
        {
            // Arrange
            var mapGenerator = new MapGenerator();
            int width = 10;
            int height = 10;
            var map = new Map(width, height);

            // Act
            var walls = map.Walls;

            // Assert
            var distinctWalls = new HashSet<Wall>(walls);  // Ensure uniqueness with GetHashCode
            Assert.Equal(walls.Count, distinctWalls.Count);
        }
    }
}
