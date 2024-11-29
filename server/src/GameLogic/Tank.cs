namespace Thuai.Server.GameLogic;

/// <summary>
/// Character controlled by a player.
/// </summary>
public class Tank
{
    public int ID { get; set; }
    public double Speed { get; set; }

    public Position TankPosition { get; set; } = new();

    public Weapon TankWeapon { get; set; } = new();

    public Armor TankArmor { get; set; } = new();

    public List<Skill> TankSkills { get; set; } = new();

    public void PerformMove(string direction)
    {
        double XChange = Speed * Math.Cos(TankPosition.Angle);
        double YChange = Speed * Math.Sin(TankPosition.Angle);
        if (direction == "BACK")
        {
            TankPosition.Xpos -= XChange;
            TankPosition.Ypos -= YChange;
        }
        if (direction == "FORTH")
        {
            TankPosition.Xpos += XChange;
            TankPosition.Ypos += YChange;
        }
    }

    public void PerformTurn(string direction)
    {
        double AngleChange = 10;
        if (direction == "CLOCKWISE")
        {
            TankPosition.Angle -= AngleChange;
        }
        if (direction == "COUNTER_CLOCKWISE")
        {
            TankPosition.Angle += AngleChange;
        }
    }
}



