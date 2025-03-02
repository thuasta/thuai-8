namespace Thuai.Server.GameController;

public partial class GameRunner
{
    public class AfterPlayerConnectEventArgs(string token, Guid socketId) : EventArgs
    {
        public string Token { get; } = token;
        public Guid SocketId { get; } = socketId;
    }
    public event EventHandler<AfterPlayerConnectEventArgs> AfterPlayerConnectEvent = delegate { };
}
