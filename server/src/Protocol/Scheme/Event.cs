using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

public record BattleUpdateEvent
{
    [JsonPropertyName("eventType")]
    public virtual string EventType { get; init; } = "";
}

public record PlayerUpdateEvent : BattleUpdateEvent
{
    [JsonPropertyName("eventType")]
    public override string EventType { get; init; } = "PLAYER_UPDATE_EVENT";

    [JsonPropertyName("player")]
    public required Player Player { get; init; }
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
