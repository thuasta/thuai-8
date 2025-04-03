using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class PositionTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainPrecision()
    {
        // Arrange
        var original = new Position
        {
            X = 12.345,
            Y = -67.890,
            Angle = 45.5
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Position>(json);

        // Assert
        Assert.Equal(original, deserialized);
        // Assert.Equal(original.X, deserialized?.X, precision: 3); // 验证3位小数精度
        // Assert.Equal(original.Y, deserialized?.Y, precision: 3);
        // Assert.Equal(original.Angle, deserialized?.Angle, precision: 3);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "x": 12.345,
            "y": -67.890,
            "angle": 45.5
        }
        """;

        // Act
        var position = JsonSerializer.Deserialize<Position>(json);

        // Assert
        Assert.Equal(12.345, position?.X);
        Assert.Equal(-67.890, position?.Y);
        Assert.Equal(45.5, position?.Angle);
    }
}