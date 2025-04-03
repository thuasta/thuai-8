using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class WallTests
{
    // 测试基本结构
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainStructure()
    {
        // Arrange
        var original = new Wall
        {
            X = 5, 
            Y = 10, 
            Angle = 45.0
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Wall>(json);

        // Assert
        Assert.Equal(original, deserialized);
        // Assert.Equal(original.Position.X, deserialized?.Position.X);
        // Assert.Equal(original.Position.Y, deserialized?.Position.Y);
        // Assert.Equal(original.Position.Angle, deserialized?.Position.Angle);
    }   

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "x": 5,
            "y": -10,
            "angle": 90
        }
        """;

        // Act
        var wall = JsonSerializer.Deserialize<Wall>(json);

        // Assert
        Assert.Equal(5, wall?.X);
        Assert.Equal(-10, wall?.Y);
        Assert.Equal(90, wall?.Angle);
    }
}
