namespace Thuai.Server.GameLogic;

public enum ArmorKnife
{
    NOT_OWNED,
    AVAILABLE,
    ACTIVE,
    BROKEN
}

public class Armor
{
    public bool CanReflect { get; set; } = false;
    public int MaximumArmorValue { get; set; } = Constants.INITIAL_ARMOR_VALUE;
    public int ArmorValue { get; set; } = Constants.INITIAL_ARMOR_VALUE;
    public int MaximumHealth { get; set; } = Constants.INITIAL_HEALTH_VALUE;
    public int Health { get; set; } = Constants.INITIAL_HEALTH_VALUE;
    public bool GravityField { get; set; } = false;
    public ArmorKnife Knife = ArmorKnife.NOT_OWNED;
    public int DodgeRate { get; set; } = Constants.INITIAL_DODGE_PERCENTAGE;

    public void Recover()
    {
        ArmorValue = MaximumArmorValue;
        Health = MaximumHealth;
        if (Knife != ArmorKnife.NOT_OWNED)
        {
            Knife = ArmorKnife.AVAILABLE;
        }
    }
}
