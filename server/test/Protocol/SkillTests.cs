using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class SkillTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var original = new Skill
        {
            Name = "Fireball",
            MaxCooldown = 10,
            CurrentCooldown = 5,
            IsActive = true
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Skill>(json);

        // Assert
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "name": "Fireball",
            "maxCooldown": 10,
            "currentCooldown": 5,
            "isActive": true
        }
        """;

        // Act
        var skill = JsonSerializer.Deserialize<Skill>(json);

        // Assert
        Assert.Equal("Fireball", skill?.Name);
        Assert.Equal(10, skill?.MaxCooldown);
        Assert.Equal(5, skill?.CurrentCooldown);
        Assert.True(skill?.IsActive);
    }
}