using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Thuai.GameServer.Recorder;

public record playerType
{

    [JsonPropertyName("token")]
    public string? token { get; init; }
    public record weaponType
    {
        [JsonPropertyName("attackSpeed")]
        public double? attackSpeed { get; set; }
        [JsonPropertyName("bulletSpeed")]
        public double? bulletSpeed { get; set; }
        [JsonPropertyName("isLaser")]
        public bool? isLaser { get; set; }
        [JsonPropertyName("antiArmor")]
        public bool? antiArmor { get; set; }
        [JsonPropertyName("damage")]
        public int? damage { get; set; }
        [JsonPropertyName("maxBullets")]
        public int? maxBullets { get; set; }
        [JsonPropertyName("currentBullets")]
        public int? currentBullets { get; set; }
    }
    public record armorType
    {
        [JsonPropertyName("canReflect")]
        public bool? canReflect { get; set; }
        [JsonPropertyName("armorValue")]
        public int? armorValue { get; set; }
        [JsonPropertyName("health")]
        public int? health { get; set; }
        [JsonPropertyName("gravityField")]
        public bool? gravityField { get; set; }
        [JsonPropertyName("knife")]
        public string? knife { get; set; }
        [JsonPropertyName("dodgeRate")]
        public double? dodgeRate { get; set; }
    }
    public record skillType
    {
        [JsonPropertyName("name")]
        public string? name { get; set; }
        [JsonPropertyName("maxCoolDown")]
        public int? maxCoolDown { get; set; }
        [JsonPropertyName("currentCoolDown")]
        public int? currentCoolDown { get; set; }
        [JsonPropertyName("isActive")]
        public bool? isActive { get; set; }
    }
    public record positionType
    {
        [JsonPropertyName("x")]
        public double x { get; set; }
        [JsonPropertyName("y")]
        public double y { get; set; }
        [JsonPropertyName("angle")]
        public double angle { get; set; }
    }
} 