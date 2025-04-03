using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class PlayerTests
{
    // Helper method to create valid nested objects
    private static Weapon CreateTestWeapon() => new Weapon
    {
        AttackSpeed = 2.5,
        BulletSpeed = 10.0,
        IsLaser = true,
        AntiArmor = false,
        Damage = 50,
        MaxBullets = 30,
        CurrentBullets = 15
    };

    private static Armor CreateTestArmor() => new Armor
    {
        CanReflect = true,
        ArmorValue = 100,
        Health = 200,
        GravityField = false,
        Knife = "AVAILABLE",
        DodgeRate = 0.75
    };

    private static List<Skill> CreateTestSkills() => new List<Skill>
    {
        new Skill { Name = "Fireball",
                    MaxCooldown = 10,
                    CurrentCooldown = 5,
                    IsActive = true }
    };

    private static Position CreateTestPosition() => new Position
    {
        X = 5,
        Y = 10
    };

    [Fact]
    public void SerializeAndDeserialize_ShouldMaintainFullDataFidelity()
    {
        // Arrange
        var original = new Player
        {
            Token = "player_123",
            Weapon = CreateTestWeapon(),
            Armor = CreateTestArmor(),
            Skills = CreateTestSkills(),
            Position = CreateTestPosition()
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Player>(json);

        // Assert
        // Assert.Equal(original, deserialized);
        Assert.Equal(original.Token, deserialized?.Token);
        Assert.Equal(original.Armor.CanReflect, deserialized?.Armor.CanReflect);
        Assert.Equal(original.Armor.ArmorValue, deserialized?.Armor.ArmorValue);
        Assert.Equal(original.Armor.Health, deserialized?.Armor.Health);
        Assert.Equal(original.Armor.GravityField, deserialized?.Armor.GravityField);
        Assert.Equal(original.Armor.Knife, deserialized?.Armor.Knife);
        Assert.Equal(original.Armor.DodgeRate, deserialized?.Armor.DodgeRate);

        Assert.Equal(original.Skills.Count, deserialized?.Skills.Count);
        Assert.Equal(original.Position.X, deserialized?.Position.X);

        Assert.Equal(original.Weapon.AttackSpeed, deserialized?.Weapon.AttackSpeed);
        Assert.Equal(original.Weapon.BulletSpeed, deserialized?.Weapon.BulletSpeed);
        Assert.Equal(original.Weapon.IsLaser, deserialized?.Weapon.IsLaser);
        Assert.Equal(original.Weapon.AntiArmor, deserialized?.Weapon.AntiArmor);
        Assert.Equal(original.Weapon.Damage, deserialized?.Weapon.Damage);
        Assert.Equal(original.Weapon.MaxBullets, deserialized?.Weapon.MaxBullets);
        Assert.Equal(original.Weapon.CurrentBullets, deserialized?.Weapon.CurrentBullets);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "token": "player_123",
            "weapon": { 
            "attackSpeed": 2.5,
            "bulletSpeed": 10.0,
            "damage": 50,
            "isLaser": true,
            "antiArmor": false,
            "weaponType": "",
            "maxBullets": 30,
            "currentBullets": 15 },
            "armor": {
            "canReflect": true,
            "armorValue": 100,
            "health": 200,
            "gravityField": false,
            "knife": "AVAILABLE",
            "dodgeRate": 0.82 },
            "skills": [{
            "name": "Fireball",
            "maxCooldown": 10,
            "currentCooldown": 5,
            "isActive": true }],
            "position": {
            "x": 12.345,
            "y": -67.890,
            "angle": 45.5
            }
        }
        """;

        // Act
        var player = JsonSerializer.Deserialize<Player>(json);

        // Assert
        Assert.Equal("player_123", player?.Token);
        Assert.True(player?.Armor.CanReflect);
        Assert.Equal(100, player?.Armor.ArmorValue);
        Assert.Equal(200, player?.Armor.Health);
        Assert.False(player?.Armor.GravityField);
        Assert.Equal("AVAILABLE", player?.Armor.Knife);
        Assert.Equal(0.82, player?.Armor.DodgeRate);

        Assert.Equal(1, player?.Skills.Count);
        Assert.Equal("Fireball", player?.Skills[0].Name);
        Assert.Equal(10, player?.Skills[0].MaxCooldown);
        Assert.Equal(5, player?.Skills[0].CurrentCooldown);
        Assert.True(player?.Skills[0].IsActive);
        Assert.Equal(12.345, player?.Position.X);
        Assert.Equal(-67.890, player?.Position.Y);
        Assert.Equal(45.5, player?.Position.Angle);

        Assert.Equal(2.5, player?.Weapon.AttackSpeed);
        Assert.Equal(10.0, player?.Weapon.BulletSpeed);
        Assert.Equal(true ,player?.Weapon.IsLaser);
        Assert.Equal(false, player?.Weapon.AntiArmor);
        Assert.Equal(50, player?.Weapon.Damage);
        Assert.Equal(30, player?.Weapon.MaxBullets);
        Assert.Equal(15, player?.Weapon.CurrentBullets);
    }
}