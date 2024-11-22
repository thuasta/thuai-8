using System.Text.Json.Serialization;

namespace GameServer.Connection;

public record BuffSelect : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BUFF_SELECT";

    [JsonPropertyName("currentTicks")]
    public int CurrentTicks { get; init; } = 0;

    [JsonPropertyName("token")]
    public string Token { get; init; } = "";

    [JsonPropertyName("buff")]
    public string Buff { get; init; } = "";
}