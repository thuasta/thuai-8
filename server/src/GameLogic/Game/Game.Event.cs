namespace Thuai.Server.GameLogic;

public partial class Game
{
    public class AfterGameTickEventArgs(Game game) : EventArgs
    {
        public Game Game => game;
    }

    public event EventHandler<AfterGameTickEventArgs> AfterGameTickEvent = delegate { };
}
