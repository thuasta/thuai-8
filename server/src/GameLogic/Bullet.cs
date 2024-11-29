namespace Thuai.Server.GameLogic;

public class Bullet{
    public Position BulletPosition{get;set;}=new();
    public double BulletSpeed;
    public double BulletDamage;
    public double BulletTraveledDistance;
}