using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class ArmorTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldBeEquivalent()
    {
        
        // Arrange
        var original = new Armor
        {
            CanReflect = true,
            ArmorValue = 100,
            Health = 200,
            GravityField = false,
            Knife = "AVAILABLE",
            DodgeRate = 0.75
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Armor>(json);

        // Assert
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "canReflect": true,
            "armorValue": 100,
            "health": 200,
            "gravityField": false,
            "knife": "AVAILABLE",
            "dodgeRate": 0.82
        }
        """;
    
        // Act
        var armor = JsonSerializer.Deserialize<Armor>(json);
    
        // Assert
        Assert.True(armor?.CanReflect);
        Assert.Equal(100, armor?.ArmorValue);
        Assert.Equal(200, armor?.Health);
        Assert.False(armor?.GravityField);
        Assert.Equal("AVAILABLE", armor?.Knife);
        Assert.Equal(0.82, armor?.DodgeRate);
    }
}