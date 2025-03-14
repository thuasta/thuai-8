using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record AllPlayerInfoMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "ALL_PLAYER_INFO";

    [JsonPropertyName("players")]
    public List<Player> Players { get; init; } = [];

}