using System.Text.Json.Serialization;

namespace GameServer.Connection;

public abstract record PerformMessage : Message
{
    [JsonPropertyName("token")]
    public string Token { get; init; } = "";
}

public record PerformMove : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_MOVE";

    [JsonPropertyName("direction")]
    public string Direction { get; init; } = "";
}

public record PerformTurn: PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_TURN";

    [JsonPropertyName("direction")]
    public string Direction { get; init; } = "";
}

public record PerformAttack : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_ATTACK";
}

public record PerformSkill : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_SKILL";

    [JsonPropertyName("skillName")]
    public string skillName { get; init; } = "";
}

public record PerformSelect : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_SELECT";

    [JsonPropertyName("buffName")]
    public string BuffName { get; init; } = "";
}

public record GetPlayerinfo : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "GET_PLAYER_INFO";

    [JsonPropertyName("request")]
    public string request { get; init; } = "";
}

public record GetEnvironmentInfo : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "Get_ENVIRONMENT_INFO";

}

public record GetGameStatistics : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "GET_GAME_STATISTICS";
}

public record GetAvailableBuffs : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "GET_AVAILABLE_BUFFS";
}