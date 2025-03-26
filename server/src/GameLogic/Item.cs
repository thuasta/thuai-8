namespace Thuai.Server.GameLogic
{
    public class Item
    {
        public enum ItemType
        {
            None,
            HealthPotion,
            ArmorUpgrade
        }

        public ItemType Type { get; }

        public Item()
        {
            Type = ItemType.None;
        }
    }
}
