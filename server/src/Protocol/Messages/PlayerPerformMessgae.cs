using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public abstract record PerformMessage : Message
{
    [JsonPropertyName("token")]
    public string Token { get; init; } = "";
}

public record PerformMoveMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_MOVE";

    [JsonPropertyName("direction")]
    public string Direction { get; init; } = "";

    [JsonPropertyName("distance")]
    public double Distance { get; init; } = 0;
}

public record PerformTurnMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_TURN";

    [JsonPropertyName("direction")]
    public string Direction { get; init; } = "";

    [JsonPropertyName("angle")]
    public int Angle { get; init; } = 0;    // In degrees
}

public record PerformAttackMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_ATTACK";
}

public record PerformSkillMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_SKILL";

    [JsonPropertyName("skillName")]
    public string SkillName { get; init; } = "";
}

public record PerformSelectMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "PERFORM_SELECT";

    [JsonPropertyName("buffName")]
    public string BuffName { get; init; } = "";
}

public record GetPlayerinfoMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "GET_PLAYER_INFO";

    [JsonPropertyName("request")]
    public string Request { get; init; } = "";
}

public record GetEnvironmentInfoMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "Get_ENVIRONMENT_INFO";

}

public record GetGameStatisticsMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "GET_GAME_STATISTICS";
}

public record GetAvailableBuffsMessage : PerformMessage
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "GET_AVAILABLE_BUFFS";
}
