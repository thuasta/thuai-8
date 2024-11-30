using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Thuai.Server.GameLogic;

namespace Thuai.GameServer.Recorder;

public record eventType
{
    [JsonPropertyName("eventType")]
    public string? messageType { get; init; }

}

public record AppearEvent : eventType
{
    [JsonPropertyName("eventType")]
    public new string messageType => "Appear";
    [JsonPropertyName("target")]
    public string? target { get; set; }
    [JsonPropertyName("position")]
    public required Position position { get; set; }
}

public record BobEvent : eventType
{
    [JsonPropertyName("eventType")]
    public new string messageType => "Bob";
    [JsonPropertyName("target")]
    public string? target { get; set; }
    [JsonPropertyName("end")]
    public required Position end { get; set; }
}

public record CollisionEvent : eventType
{
    [JsonPropertyName("eventType")]
    public new string messageType => "Collision";
    [JsonPropertyName("target")]
    public required List<string> targets { get; set; }
}

public record DestroyEvent : eventType
{
    [JsonPropertyName("eventType")]
    public new string messageType => "Destroy";
    [JsonPropertyName("target")]
    public string? target { get; set; }
}