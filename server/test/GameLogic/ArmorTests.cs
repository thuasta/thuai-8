using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic;

//Checked Original Tests 03/17/2025
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
        Assert.False(armor.CanReflect);
        Assert.Equal(Constants.INITIAL_ARMOR_VALUE, armor.MaximumArmorValue);
        Assert.Equal(Constants.INITIAL_ARMOR_VALUE, armor.ArmorValue);
        Assert.Equal(Constants.INITIAL_HEALTH_VALUE, armor.Health);
        Assert.Equal(Constants.INITIAL_HEALTH_VALUE, armor.MaximumHealth);
        Assert.False(armor.GravityField);
        // Assert.Equal(ArmorKnife.NOT_OWNED, armor.Knife);
        Assert.Equal(Constants.INITIAL_DODGE_PERCENTAGE, armor.DodgeRate);
    }

    // [Theory]
    // [InlineData(0, 0, ArmorKnife.ACTIVE)]
    // [InlineData(0, 0, ArmorKnife.BROKEN)]
    // [InlineData(1, 1, ArmorKnife.ACTIVE)]
    // public void Recover_ArmorOwned_KnifeIsAvailable(int armorValue, int health, ArmorKnife knife)
    // {
    //     // Arrange.
    //     Armor armor = new()
    //     {
    //         ArmorValue = armorValue,
    //         Health = health,
    //         Knife = knife
    //     };

    //     // Act.
    //     armor.Recover();

    //     // Assert.
    //     Assert.Equal(Constants.INITIAL_ARMOR_VALUE, armor.ArmorValue);
    //     Assert.Equal(Constants.INITIAL_HEALTH_VALUE, armor.Health);
    //     Assert.Equal(ArmorKnife.AVAILABLE, armor.Knife);
    // }

    // [Fact]
    // public void Recover_ArmorNotOwned_KnifeIsNotOwned()
    // {
    //     // Arrange.
    //     Armor armor = new()
    //     {
    //         Knife = ArmorKnife.NOT_OWNED
    //     };

    //     // Act.
    //     armor.Recover();

    //     // Assert.
    //     Assert.Equal(ArmorKnife.NOT_OWNED, armor.Knife);
    // }
}
