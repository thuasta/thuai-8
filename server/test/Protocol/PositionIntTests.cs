using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class PositionIntTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainPrecision()
    {
        // Arrange
        var original = new PositionInt
        {
            X = 5,
            Y = -10,
            Angle = 90
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<PositionInt>(json);

        // Assert
        Assert.Equal(original, deserialized);
        // Assert.Equal(original.X, deserialized?.X);
        // Assert.Equal(original.Y, deserialized?.Y);
        // Assert.Equal(original.Angle, deserialized?.Angle);
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
        var positionInt = JsonSerializer.Deserialize<PositionInt>(json);

        // Assert
        Assert.Equal(5, positionInt?.X);
        Assert.Equal(-10, positionInt?.Y);
        Assert.Equal(90, positionInt?.Angle);
    }
}