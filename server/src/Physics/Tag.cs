namespace Thuai.Server.Physics;

public class Tag
{
    public required object Owner { get; init; }
    public Dictionary<Key, object> AttachedData { get; } = [];
}

public enum Key
{
    CoveredFields,
    SpeedUpFactor,
    CorrespondingWallPosition
}
