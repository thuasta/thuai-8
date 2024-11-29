namespace Thuai.Server.GameLogic;

public interface IObject
{
    public enum ObjectType
    {
        Player,
        Wall,
        Fence,
        Bullet,
        Laser,
        BuffName
    }

    public String ToString();
}