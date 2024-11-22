using System.Text.Json.Serialization;

namespace GameServer.Connection;

public record Error : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "ERROR";

    [JsonPropertyName("errorCode")]
    public int ErrorCode { get; init; } = 0;

    [JsonPropertyName("message")]
    public string Message { get; init; } = "";
}