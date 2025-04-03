using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class LaserTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainStructuralEquality()
    {
        // Arrange
        var original = new Laser
        {
            Start = new Position { X = 5, Y = 10 },
            End = new Position { X = 15, Y = 20 }
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Laser>(json);

        // Assert
        Assert.Equal(original, deserialized);
        // Assert.Equal(original.Start.X, deserialized?.Start.X);
        // Assert.Equal(original.Start.Y, deserialized?.Start.Y);
        // Assert.Equal(original.End.X, deserialized?.End.X);
        // Assert.Equal(original.End.Y, deserialized?.End.Y);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "start": { "x": 5, "y": 10 },
            "end": { "x": 15, "y": 20}
        }
        """;

        // Act
        var laser = JsonSerializer.Deserialize<Laser>(json);

        // Assert
        Assert.Equal(5, laser?.Start.X);
        Assert.Equal(10, laser?.Start.Y);
        Assert.Equal(15, laser?.End.X);
        Assert.Equal(20, laser?.End.Y);
    }

}