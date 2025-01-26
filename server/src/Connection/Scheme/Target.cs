using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;
public record Target
{
    [JsonPropertyName("player")]
    public Player Player { get; init; } = new();

    [JsonPropertyName("position")]
    public Position Position { get; init; } = new();

    [JsonPropertyName("fence")]
    public Fence Fence { get; init; } = new();

    [JsonPropertyName("bullet")]
    public Bullet Bullet { get; init; } = new();

    [JsonPropertyName("laser")]
    public List<Laser> Laser { get; init; } = new();

    [JsonPropertyName("buffName")]
    public string BuffName { get; init; } = "";
}