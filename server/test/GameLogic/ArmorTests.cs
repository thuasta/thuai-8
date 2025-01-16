using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic;

public class ArmorTests
{
    [Fact]
    public void Fields_DefaultValues_AreCorrect()
    {
        // Arrange.
        Armor armor = new();

        // Act.
        // No need to act.

        // Assert.
        Assert.False(armor.canReflect);
        Assert.Equal(Constants.INITIAL_ARMOR_VALUE, armor.armorValue);
        Assert.Equal(Constants.INITIAL_HEALTH_VALUE, armor.health);
        Assert.False(armor.gravityField);
        Assert.Equal(ArmorKnife.NOT_OWNED, armor.knife);
        Assert.Equal(0, armor.dodgeRate);
    }
}
