namespace Thuai.GameServer.MapGenerator;

class Wall 
{
    public Wall(int x, int y, int angle)
    {
        X = x;
        Y = y;
        Angle = angle;
    }
    private int X { get; }
    private int Y { get; }
    private int Angle { get; }
}