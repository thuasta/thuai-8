using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class WeaponTests
{ 
    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainAllProperties()
    {
        // Arrange
        var original = new Weapon
        {
            AttackSpeed = 2.5,
            BulletSpeed = 10.0,
            IsLaser = true,
            AntiArmor = false,
            Damage = 50,
            MaxBullets = 30,
            CurrentBullets = 15
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Weapon>(json);

        // Assert
        Assert.Equal(original, deserialized);
        // Assert.Equal(original.AttackSpeed, deserialized?.AttackSpeed);
        // Assert.Equal(original.BulletSpeed, deserialized?.BulletSpeed);
        // Assert.Equal(original.IsLaser, deserialized?.IsLaser);
        // Assert.Equal(original.AntiArmor, deserialized?.AntiArmor);
        // Assert.Equal(original.Damage, deserialized?.Damage);
        // Assert.Equal(original.MaxBullets, deserialized?.MaxBullets);
        // Assert.Equal(original.CurrentBullets, deserialized?.CurrentBullets);
    }
    
    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "attackSpeed": 2.5,
            "bulletSpeed": 10.0,
            "damage": 50,
            "isLaser": true,
            "antiArmor": false,
            "weaponType": "",
            "maxBullets": 30,
            "currentBullets": 15
        }
        """;
    
        // Act
        var weapon = JsonSerializer.Deserialize<Weapon>(json);
    
        // Assert
        Assert.Equal(2.5, weapon?.AttackSpeed);
        Assert.Equal(10.0, weapon?.BulletSpeed);
        Assert.Equal(50, weapon?.Damage);
        Assert.Equal(30, weapon?.MaxBullets);
        Assert.Equal(15, weapon?.CurrentBullets);
        Assert.True(weapon?.IsLaser);
        Assert.False(weapon?.AntiArmor);
    }
}