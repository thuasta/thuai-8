using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

[JsonConverter(typeof(BattleUpdateEventConverter))]
public record BattleUpdateEvent
{
    [JsonPropertyName("eventType")]
    public virtual string EventType { get; init; } = "";
}

public record PlayerUpdateEvent : BattleUpdateEvent
{
    [JsonPropertyName("eventType")]
    public override string EventType { get; init; } = "PLAYER_UPDATE_EVENT";

    [JsonPropertyName("players")]
    public required List<Player> Players { get; init; }
}

public record BulletsUpdateEvent : BattleUpdateEvent
{
    [JsonPropertyName("eventType")]
    public override string EventType { get; init; } = "BULLETS_UPDATE_EVENT";

    [JsonPropertyName("bullets")]
    public required List<Bullet> Bullets { get; init; }
}

public record MapUpdateEvent : BattleUpdateEvent
{
    [JsonPropertyName("eventType")]
    public override string EventType { get; init; } = "MAP_UPDATE_EVENT";

    [JsonPropertyName("walls")]
    public required List<Wall> Walls { get; init; }

    [JsonPropertyName("fences")]
    public required List<Fence> Fences { get; init; }

    [JsonPropertyName("traps")]
    public required List<Trap> Traps { get; init; }

    [JsonPropertyName("laser")]
    public required List<Laser> Laser { get; init; }
}

public record BuffActivateEvent : BattleUpdateEvent
{
    [JsonPropertyName("eventType")]
    public override string EventType { get; init; } = "BUFF_ACTIVE_EVENT";

    [JsonPropertyName("buffName")]
    public required string BuffName { get; init; }

    [JsonPropertyName("playerToken")]
    public required string PlayerToken { get; init; }
}

// The typo exists in API documentation, so we keep it here.
public record BuffDisactivateEvent : BattleUpdateEvent
{
    [JsonPropertyName("eventType")]
    public override string EventType { get; init; } = "BUFF_DISACTIVE_EVENT";

    [JsonPropertyName("buffName")]
    public required string BuffName { get; init; }

    [JsonPropertyName("playerToken")]
    public required string PlayerToken { get; init; }
}

public class BattleUpdateEventConverter : JsonConverter<BattleUpdateEvent>
{
    public override BattleUpdateEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var root = jsonDocument.RootElement;
        var eventType = root.GetProperty("eventType").GetString()
            ?? throw new InvalidOperationException("EventType is missing");

        return eventType switch
        {
            "PLAYER_UPDATE_EVENT" => JsonSerializer.Deserialize<PlayerUpdateEvent>(root.GetRawText(), options)
                ?? throw new InvalidOperationException("Failed to deserialize PlayerUpdateEvent"),
            "BULLETS_UPDATE_EVENT" => JsonSerializer.Deserialize<BulletsUpdateEvent>(root.GetRawText(), options)
                ?? throw new InvalidOperationException("Failed to deserialize BulletsUpdateEvent"),
            "MAP_UPDATE_EVENT" => JsonSerializer.Deserialize<MapUpdateEvent>(root.GetRawText(), options)
                ?? throw new InvalidOperationException("Failed to deserialize MapUpdateEvent"),
            "BUFF_ACTIVE_EVENT" => JsonSerializer.Deserialize<BuffActivateEvent>(root.GetRawText(), options)
                ?? throw new InvalidOperationException("Failed to deserialize BuffActivateEvent"),
            "BUFF_DISACTIVE_EVENT" => JsonSerializer.Deserialize<BuffDisactivateEvent>(root.GetRawText(), options)
                ?? throw new InvalidOperationException("Failed to deserialize BuffDisactivateEvent"),
            _ => throw new InvalidOperationException($"Unknown event type: {(eventType == "" ? "[null]" : eventType)}")
        };
    }

    public override void Write(Utf8JsonWriter writer, BattleUpdateEvent value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case PlayerUpdateEvent playerUpdateEvent:
                JsonSerializer.Serialize(writer, playerUpdateEvent, options);
                break;
            case BulletsUpdateEvent bulletsUpdateEvent:
                JsonSerializer.Serialize(writer, bulletsUpdateEvent, options);
                break;
            case MapUpdateEvent mapUpdateEvent:
                JsonSerializer.Serialize(writer, mapUpdateEvent, options);
                break;
            case BuffActivateEvent buffActivateEvent:
                JsonSerializer.Serialize(writer, buffActivateEvent, options);
                break;
            case BuffDisactivateEvent buffDisactivateEvent:
                JsonSerializer.Serialize(writer, buffDisactivateEvent, options);
                break;
            default:
                throw new InvalidOperationException(
                    $"Unknown event type: {((value.EventType == "") ? "[null]" : value.EventType)}"
                );
        }
    }
}
