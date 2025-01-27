using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record Events
{
    [JsonPropertyName("messageType")]

    public virtual string MessageType { get; init; } = "";
}

public record AppearEvent : Events
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "APPEAR_EVENT";

    [JsonPropertyName("target")]
    public Target Target { get; init; } = new();

    [JsonPropertyName("position")]
    public Position Position { get; init; } = new();
}

public record BobEvent : Events
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BOB_EVENT";

    [JsonPropertyName("target")]
    public Target Target { get; init; } = new();

    [JsonPropertyName("end")]
    public Position End { get; init; } = new();
}

public record CollisionEvent : Events
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "COLLISION_EVENT";

    [JsonPropertyName("targets")]
    public List<Target> Targets { get; init; } = new();
}

public record DestoryEvent : Events
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "DESTORY_EVENT";

    [JsonPropertyName("target")]
    public Target Target { get; init; } = new();
}