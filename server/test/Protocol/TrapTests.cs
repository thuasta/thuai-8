using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class TrapTests
{
    // Helper method to create valid PositionInt
    private static PositionInt CreateTestPosition() => new()
    {
        X = 5,
        Y = 10,
        Angle = 90
    };

    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainDataConsistency()
    {
        // Arrange
        var original = new Trap
        {
            Position = CreateTestPosition(),
            IsActive = true
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Trap>(json);

        // Assert
        Assert.Equal(original.Position.X, deserialized?.Position.X);
        Assert.Equal(original.Position.Y, deserialized?.Position.Y);
        Assert.Equal(original.IsActive, deserialized?.IsActive);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "position": { "x": 5, "y": 10, "angle": 90 },
            "isActive": true
        }
        """;

        // Act
        var trap = JsonSerializer.Deserialize<Trap>(json);

        // Assert
        Assert.Equal(5, trap?.Position.X);
        Assert.Equal(10, trap?.Position.Y);
        Assert.Equal(90, trap?.Position.Angle);
        Assert.True(trap?.IsActive);
    }
}