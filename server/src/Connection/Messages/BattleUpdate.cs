using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record BattleUpdateMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BATTLE_UPDATE";

    [JsonPropertyName("currentTicks")]
    public int currentTicks { get; init; } = 0;

    [JsonPropertyName("players")]
    public List<Player> Players { get; init; } = new();

    [JsonPropertyName("events")]
    [JsonConverter(typeof(EventConverter))]
    public List<events> Events { get; init; } = new();

    public record Player
    {
        [JsonPropertyName("token")]
        public string Token { get; init; }

        [JsonPropertyName("weapon")]
        public List<weapon> Weapon { get; init; } = new();

        [JsonPropertyName("armor")]
        public armor Armor { get; init; } = new();

        [JsonPropertyName("skills")]
        public List<skill> Skills { get; init; } = new();

        [JsonPropertyName("position")]
        public position Position { get; init; } = new();
    }

    public record events
    {
        [JsonPropertyName("messageType")]

        public virtual string  MessageType{ get; init; } = "";
    }

    public record AppearEvent : events
    {
        [JsonPropertyName("messageType")]
        public override string eventType { get; init; } = "APPEAR_EVENT";

        [JsonPropertyName("target")]
        public target Target { get; init; } = new(); 

        [JsonPropertyName("position")]
        public position Position { get; init; } = new();
    }

    public record BobEvent : events
    {
        [JsonPropertyName("messageType")]
        public override string MessageType { get; init; } = "BOB_EVENT";

        [JsonPropertyName("target")]
        public target Target { get; init; } = new();

        [JsonPropertyName("end")]
        public position End { get; init; } = new();
    }

    public record CollisionEvent : events
    {
        [JsonPropertyName("messageType")]
        public override string MessageType { get; init; } = "COLLISION_EVENT";

        [JsonPropertyName("targets")]
        public List<target> Targets { get; init; } = new();
    }

    public record DestoryEvent : events
    {
        [JsonPropertyName("messageType")]
        public override string MessageType { get; init; } = "DESTORY_EVENT";

        [JsonPropertyName("target")]
        public target Target { get; init; } = new();
    }

    public class EventConverter : JsonConverter<events>
    {
        public override events? Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement jsonObject = JsonDocument.ParseValue(ref reader).RootElement;

            string? eventType = jsonObject.GetProperty("messageTYpe").GetString();

            return eventType switch
            {
                "APPEAR_EVENT" => JsonSerializer.Deserialize<AppearEvent>(jsonObject.GetRawText(), options),
                "BOB_EVENT" => JsonSerializer.Deserialize<BobEvent>(jsonObject.GetRawText(), options),
                "COLLISION_EVENT" => JsonSerializer.Deserialize<CollisionEvent>(jsonObject.GetRawText(), options),
                "DESTORY_EVENT" => JsonSerializer.Deserialize<DestoryEvent>(jsonObject.GetRawText(), options),
                _ => throw new NotSupportedException(),
            };
        }

        public override void Write(
            Utf8JsonWriter writer, events value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }

    public record target
    {
        [JsonPropertyName("player")]
        public player Player { get; init; } = new();

        [JsonPropertyName("position")]
        public position Position { get; init; } = new();

        [JsonPropertyName("fence")]
        public fence Fence { get; init; } = new();

        [JsonPropertyName("bullet")]
        public bullet Bullet {get; init; } = new();

        [JsonPropertyName("laser")]
        public List<laser> Laser { get; init; } = new();

        [JsonPropertyName("buffName")]
        public string BuffName { get; init; } = "";

        public record player
        {
            [JsonPropertyName("token")]
            public string Token { get; init; } = "";

            [JsonPropertyName("weapon")]
            public weapon Weapon { get; init; } = new();

            [JsonPropertyName("armor")]
            public armor Armor { get; init; } = new();

            [JsonPropertyName("skills")]
            public List<skill> Skills { get; init; } = new();

            [JsonPropertyName("position")]
            public position Position { get; init; } = new();
        }

        public record fence {
            [JsonPropertyName("position")]
            public position Position { get; init; } = new();

            [JsonPropertyName("health")]
            public int Health { get; init; } = 0;
        }

        public record bullet {
            [JsonPropertyName("position")]
            public position Position { get; init; } = new();

            [JsonPropertyName("speed")]
            public double Speed { get; init; }

            [JsonPropertyName("damage")]
            public double Damage { get; init; } = 0;

            [JsonPropertyName("traveledDistance")]
            public double TraveledDistance { get; init; } = 0;
        }

        public record laser {
            [JsonPropertyName("start")]
            public position start { get; init; } = new();

            [JsonPropertyName("end")]
            public position End { get; init; }
        }
    }
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