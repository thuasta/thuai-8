using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thuai.Server.Connection;

public record BattleUpdateMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "BATTLE_UPDATE";

    [JsonPropertyName("currentTicks")]
    public int currentTicks { get; init; } = 0;

    [JsonPropertyName("players")]
    public List<Player> Players { get; init; } = new();

    [JsonPropertyName("events")]
    [JsonConverter(typeof(EventConverter))]
    public List<Events> Events { get; init; } = new();

    public class EventConverter : JsonConverter<Events>
    {
        public override Events? Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement jsonObject = JsonDocument.ParseValue(ref reader).RootElement;

            string? eventType = jsonObject.GetProperty("messageTYpe").GetString();

            return eventType switch
            {
                "APPEAR_EVENT" => JsonSerializer.Deserialize<AppearEvent>(jsonObject.GetRawText(), options),
                "BOB_EVENT" => JsonSerializer.Deserialize<BobEvent>(jsonObject.GetRawText(), options),
                "COLLISION_EVENT" => JsonSerializer.Deserialize<CollisionEvent>(jsonObject.GetRawText(), options),
                "DESTORY_EVENT" => JsonSerializer.Deserialize<DestoryEvent>(jsonObject.GetRawText(), options),
                _ => throw new NotSupportedException(),
            };
        }

        public override void Write(
            Utf8JsonWriter writer, Events value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}