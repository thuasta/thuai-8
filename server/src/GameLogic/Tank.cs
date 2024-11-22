namespace Thuai.Server.GameLogic;

public class Position
{
    public double Xpos { get; set; }
    public double Ypos { get; set; }

    public double Angle{get;set;}
}

public class Tank
{
    public int ID { get; set; }
    public double Speed { get; set; }

    public Position TankPosition{get;set;}=new();

    public Weapon TankWeapon{get;set;}=new();

    public Armor TankArmor{get;set;}=new();

    public List<Skill> TankSkills{get;set;}=new();
}