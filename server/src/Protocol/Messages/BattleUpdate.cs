using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Messages;

public record BattleUpdateMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BATTLE_UPDATE";

    [JsonPropertyName("currentTicks")]
    public int CurrentTicks { get; init; } = 0;

    [JsonPropertyName("players")]
    public List<Scheme.Player> Players { get; init; } = [];

    [JsonPropertyName("events")]
    [JsonConverter(typeof(EventConverter))]
    public List<Scheme.Events> Events { get; init; } = [];

    public class EventConverter : JsonConverter<Scheme.Events>
    {
        public override Scheme.Events? Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement jsonObject = JsonDocument.ParseValue(ref reader).RootElement;

            string? eventType = jsonObject.GetProperty("messageType").GetString();

            return eventType switch
            {
                "APPEAR_EVENT" => JsonSerializer.Deserialize<Scheme.AppearEvent>(jsonObject.GetRawText(), options),
                "BOB_EVENT" => JsonSerializer.Deserialize<Scheme.BobEvent>(jsonObject.GetRawText(), options),
                "COLLISION_EVENT" => JsonSerializer.Deserialize<Scheme.CollisionEvent>(jsonObject.GetRawText(), options),
                "DESTORY_EVENT" => JsonSerializer.Deserialize<Scheme.DestoryEvent>(jsonObject.GetRawText(), options),
                _ => throw new NotSupportedException(),
            };
        }

        public override void Write(
            Utf8JsonWriter writer, Scheme.Events value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}