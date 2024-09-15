namespace Thuai.Server.GameLogic;

/// <summary>
/// Represents an item in the game.
/// </summary>
public class Item
{
    public enum ItemType
    {
        // TODO: Implement
    }

    /// <summary>
    /// The type of the item.
    /// </summary>
    public ItemType Type { get; }

    /// <summary>
    /// The collision box of the item.
    /// </summary>
    public Geometry.Shapes.Rectangle CollisionBox { get; }

    public Item(ItemType type, Geometry.Shapes.Rectangle collisionBox)
    {
        Type = type;
        CollisionBox = collisionBox;
    }
}
