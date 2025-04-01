using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class BulletTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainEquivalence()
    {
        // Arrange
        var original = new Bullet
        {
            Position = new Position { X = 5, Y = 10 },
            Speed = 2.5,
            Damage = 50.0,
            TraveledDistance = 100.0
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Bullet>(json);

        // Assert
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "position": { "x": 5, "y": 10 },
            "speed": 2.5,
            "damage": 50.0,
            "traveledDistance": 100.0
        }
        """;
    
        // Act
        var bullet = JsonSerializer.Deserialize<Bullet>(json);
    
        // Assert
        Assert.Equal(5, bullet?.Position.X);
        Assert.Equal(10, bullet?.Position.Y);
        Assert.Equal(2.5, bullet?.Speed);
        Assert.Equal(50.0, bullet?.Damage);
        Assert.Equal(100.0, bullet?.TraveledDistance);
    }
}