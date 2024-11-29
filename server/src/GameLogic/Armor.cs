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
    public int armorValue;
    public int health;
    public bool gravityField;
    public ArmorKnife knife;
    public double dodgeRate;
}