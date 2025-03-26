using Thuai.Server.GameLogic;

//Checked original tests 03/17/2025(although the class is not understoodable)
namespace Thuai.Server.Test.GameLogic
{
    public class ItemTests
    {
        [Fact]
        public void Item_DefaultType_IsCorrect()
        {
            // Arrange.
            var item = new Item();

            // Act.
            // No need to act as we are testing the default value.

            // Assert.
            // 这里假设 ItemType 目前有一个默认值，比如 ItemType.NONE（如果没有具体的值，可以改成任何其他有效的默认值）
            Assert.Equal(Item.ItemType.None, item.Type);
        }
    }
}
