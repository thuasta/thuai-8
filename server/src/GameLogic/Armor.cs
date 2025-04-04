namespace Thuai.Server.GameLogic;

public enum ArmorKnifeState
{
    NOT_OWNED,
    AVAILABLE,
    ACTIVE,
    BROKEN
}

public class ArmorKnife
{
    public ArmorKnifeState State { get; private set; } = ArmorKnifeState.NOT_OWNED;
    public bool IsAvailable => State == ArmorKnifeState.AVAILABLE;
    public bool IsActivated => State == ArmorKnifeState.ACTIVE;

    private readonly Counter _remainingTime = new(Constants.KNIFE_REMAINING_TIME);

    public void Acquire()
    {
        if (State == ArmorKnifeState.NOT_OWNED)
        {
            State = ArmorKnifeState.AVAILABLE;
            _remainingTime.Clear();
        }
    }
    public void Recover()
    {
        if (State != ArmorKnifeState.NOT_OWNED)
        {
            State = ArmorKnifeState.AVAILABLE;
            _remainingTime.Clear();
        }
    }
    public void Activate()
    {
        if (State == ArmorKnifeState.AVAILABLE)
        {
            State = ArmorKnifeState.ACTIVE;
            _remainingTime.Reset();
        }
    }
    public void Update()
    {
        if (State == ArmorKnifeState.ACTIVE)
        {
            _remainingTime.Decrease();
            if (_remainingTime.IsZero)
            {
                State = ArmorKnifeState.BROKEN;
            }
        }
    }
}

public class Armor
{
    public bool CanReflect { get; set; } = false;
    public int MaximumArmorValue { get; set; } = Constants.INITIAL_ARMOR_VALUE;
    public int ArmorValue { get; set; } = Constants.INITIAL_ARMOR_VALUE;
    public int MaximumHealth { get; set; } = Constants.INITIAL_HEALTH_VALUE;
    public int Health { get; set; } = Constants.INITIAL_HEALTH_VALUE;
    public bool GravityField { get; set; } = false;
    public ArmorKnife Knife = new();
    public int DodgeRate { get; set; } = Constants.INITIAL_DODGE_PERCENTAGE;    // In percentage

    public void Recover()
    {
        ArmorValue = MaximumArmorValue;
        Health = MaximumHealth;
        Knife.Recover();
    }
}
