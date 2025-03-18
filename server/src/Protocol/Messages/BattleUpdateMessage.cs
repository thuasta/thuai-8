using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record BattleUpdateMessage : Message, IRecordable
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BATTLE_UPDATE";

    [JsonPropertyName("battleTicks")]
    public int BattleTicks { get; init; } = 0;

    [JsonPropertyName("events")]
    public List<Scheme.BattleUpdateEvent> Events { get; init; } = [];
}