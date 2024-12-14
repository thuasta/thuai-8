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
    public bool canReflect;
    public double armorValue;
    public double health;
    public bool gravityField;
    public ArmorKnife knife;
    public double dodgeRate;

    public Armor()
    {
        this.canReflect = false;
        this.armorValue = Constants.INITIAL_ARMOR_VALUE;
        this.health = Constants.INITIAL_HEALTH_VALUE;
        this.gravityField = false;
        this.knife = ArmorKnife.NOT_OWNED;
        this.dodgeRate = 0;
    }
}