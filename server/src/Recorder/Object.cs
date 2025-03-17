namespace Thuai.Server.Recorder;

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

    public string ToString();
}