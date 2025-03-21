using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record AllPlayerInfoMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "ALL_PLAYER_INFO";

    [JsonPropertyName("players")]
    public List<Scheme.Player> Players { get; init; } = [];

}