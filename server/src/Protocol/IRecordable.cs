using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol;

[JsonConverter(typeof(RecordableJsonConverter))]
public interface IRecordable
{
    // Just for marking the recordable messages.
}

public class RecordableJsonConverter : JsonConverter<IRecordable>
{
    public override IRecordable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new InvalidOperationException("IRecordable objects should not be created from a json.");
    }

    public override void Write(Utf8JsonWriter writer, IRecordable value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}
