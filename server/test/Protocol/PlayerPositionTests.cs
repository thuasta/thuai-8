using System.Runtime.CompilerServices;
using System.Text.Json;
using nkast.Aether.Physics2D.Dynamics.Joints;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class PlayerPositionTests
{
    // 测试基本结构
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainStructure()
    {
        // Arrange
        var original = new PlayerPositions
        {
            Position = new Position{ X = 0, Y = 0 },
            Token = ""
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<PlayerPositions>(json);

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
            "position": { "x": 0, "y": 0, "angle": 0},
            "token": ""
        }
        """;

        // Act
        var playerposition = JsonSerializer.Deserialize<PlayerPositions>(json);

        // Assert
        Assert.Equal(0, playerposition?.Position.X);
        Assert.Equal(0, playerposition?.Position.Y);
        Assert.Equal(0, playerposition?.Position.Angle);
        Assert.Equal("", playerposition?.Token);
    }
}
