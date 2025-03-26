using System.Text.Json.Serialization;

namespace Thuai.Server.Protocol.Scheme;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Stage
{
    REST,
    BATTLE,
    END
}