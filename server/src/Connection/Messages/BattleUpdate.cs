using System.Text.Json.Serialization;

namespace GameServer.Connection;

public record BattleUpdate : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BATTLE_UPDATE";

    [JsonPropertyName("currentTicks")]
    public int currentTicks { get; init; } = 0;

    [JsonPropertyName("players")]
    public List<Player> Players { get; init; } = new();

    [JsonPropertyName("events")]
    [JsonConverter(typeof(EventConverter))]
    public List<Event> Events { get; init; } = new();

    public record Player
    {
        [JsonPropertyName("token")]
        public string Token { get; init; }

        [JsonPropertyName("weapon")]
        public List<weapon> Weapon { get; init; } = new();

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

        public record weapon
        {
            [JsonPropertyName("attackSpeed")]
            public double AttackSpeed { get; init; };

            [JsonPropertyName("bulletSpeed")]
            public double BulletSpeed { get; init; };

            [JsonPropertyName("isLaser")]
            public bool IsLaser { get; init; };

            [JsonPropertyName("antiArmor")]
            public bool AntiArmor { get; init; };

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
            public bool CanReflect { get; init; };

            [JsonPropertyName("armorValue")]
            public int ArmorValue { get; init; } = 0;

            [JsonPropertyName("health")]
            public int Health { get; init; } = 0;

            [JsonPropertyName("gravityField")]
            public bool GravityField { get; init; };

            [JsonPropertyName("knife")]
            public string Knife { get; init; } = "";

            [JsonPropertyName("dodgeRate")]
            public double DodgeRate { get; init; };
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
            public bool IsActive { get; init; };
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

    public record events 
    {
        [JsonPropertyName("eventType")]
        public virtual string EventType { get; init; } = "";
    }
//写到这里
    public record AppearEvent : events
    {
        [JsonPropertyName("eventType")]
        public override string EventType { get; init; } = "APPEAR_EVENT";

        [JsonPropertyName("target")]
        public target Target { get; init; } = new(); 

        [JsonPropertyName("position")]
        public position Position { get; init; } = new();

        public record target
        {
            [JsonPropertyName("targetName")]
            public string TargetName { get; init; } = "";
        }

        public record position
        {
            [JsonPropertyName("undefined")]
            public double X { get; init; }

            [JsonPropertyName("Position")]
            public double Y { get; init; }

            public record Position
            {
                [JsonPropertyName("x")]
                public double X { get; init; }

                [JsonPropertyName("y")]
                public double Y { get; init; }

                [JsonPropertyName("angle")]
                public double Angle { get; init; }
            }


        }
    }

    // public class EventConverter : JsonConverter<Event>
    // {
    //     public override Event? Read(
    //         ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //     {
    //         JsonElement jsonObject = JsonDocument.ParseValue(ref reader).RootElement;

    //         string? eventType = jsonObject.GetProperty("eventType").GetString();

    //         return eventType switch
    //         {
    //             "PLAYER_ATTACK" => JsonSerializer.Deserialize<PlayerAttackEvent>(jsonObject.GetRawText(), options),
    //             "PLAYER_SWITCH_ARM" => JsonSerializer.Deserialize<PlayerSwitchArmEvent>(jsonObject.GetRawText(), options),
    //             "PLAYER_PICK_UP" => JsonSerializer.Deserialize<PlayerPickUpEvent>(jsonObject.GetRawText(), options),
    //             "PLAYER_USE_MEDICINE" => JsonSerializer.Deserialize<PlayerUseMedicineEvent>(jsonObject.GetRawText(), options),
    //             "PLAYER_USE_GRENADE" => JsonSerializer.Deserialize<PlayerUseGrenadeEvent>(jsonObject.GetRawText(), options),
    //             "PLAYER_ABANDON" => JsonSerializer.Deserialize<PlayerAbandonEvent>(jsonObject.GetRawText(), options),
    //             _ => throw new NotSupportedException(),
    //         };
    //     }

    //     public override void Write(
    //         Utf8JsonWriter writer, Event value, JsonSerializerOptions options)
    //     {
    //         JsonSerializer.Serialize(writer, (object)value, options);
    //     }
    // }
}