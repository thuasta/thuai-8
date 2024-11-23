using System.Text.Json.Serialization;

namespace GameServer.Connection;

public record PlayerInfo : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PlayerInfo";

    [JsonPropertyName("token")]
    public string Token { get; init; } = "";

    public record weapon
    {
        [JsonPropertyName("attackSpeed")]
        public double AttackSpeed { get; init; }

        [JsonPropertyName("bulletSpeed")]
        public double BulletSpeed { get; init; }

        [JsonPropertyName("isLaser")]
        public bool IsLaser { get; init; }

        [JsonPropertyName("antiArmor")]
        public bool AntiArmor { get; init; }

        [JsonPropertyName("damage")]
        public int Damage { get; init; } = 0;

        [JsonPropertyName("maxBullets")]
        public int MaxBullets { get; init; } = 0;

        [JsonPropertyName("currentBullets")]
        public int CurrentBullets { get; init; } = 0;
    }

    [JsonPropertyName("weapon")]
    public weapon Weapon { get; init; } = new();

    public record armor 
    {
        [JsonPropertyName("canReflect")]
        public bool CanReflect { get; init; }

        [JsonPropertyName("armorValue")]
        public int ArmorValue { get; init; } = 0;

        [JsonPropertyName("health")]
        public int Health { get; init; } = 0;

        [JsonPropertyName("gravityField")]
        public bool GravityField { get; init; }

        [JsonPropertyName("knife")]
        public string Knife { get; init; } = "";

        [JsonPropertyName("dodgeRate")]
        public double DodgeRate { get; init; }
    }

    [JsonPropertyName("armor")]
    public armor Armor { get; init; } = new();

    [JsonPropertyName("skills")]
    public List<skill> Skills { get; init; } = new();

    public record skill
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = "";

        [JsonPropertyName("maxCooldown")]
        public int MaxCooldown { get; init; } = 0;

        [JsonPropertyName("currentCooldown")]
        public int CurrentCooldown { get; init; } = 0;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; init; }
    }

    [JsonPropertyName("position")]
    public position Position { get; init; } = new();

    public record position
    {
        [JsonPropertyName("x")]
        public double X { get; init; }

        [JsonPropertyName("y")]
        public double Y { get; init; }

        [JsonPropertyName("angle")]
        public double Angle { get; init; }
    }

}