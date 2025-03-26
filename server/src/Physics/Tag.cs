namespace Thuai.Server.Physics;

public class Tag
{
    public required object Owner { get; init; }
    public Dictionary<string, object> AttachedData { get; } = [];
}
