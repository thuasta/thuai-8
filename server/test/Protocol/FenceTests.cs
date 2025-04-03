using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class FenceTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var original = new Fence
        {
            Position = new Position { X = 10, Y = 20 },
            Health = 100
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Fence>(json);

        // Assert
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "position": { "x": 10, "y": 20 },
            "health": 100
        }
        """;

        // Act
        var fence = JsonSerializer.Deserialize<Fence>(json);

        // Assert
        Assert.Equal(10, fence?.Position.X);
        Assert.Equal(20, fence?.Position.Y);
        Assert.Equal(100, fence?.Health);
    }
}
