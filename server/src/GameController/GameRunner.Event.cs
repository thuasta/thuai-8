namespace Thuai.Server.GameController;

public partial class GameRunner
{
    public class AfterPlayerConnectEventArgs(string token, Guid socketId) : EventArgs
    {
        public string Token { get; } = token;
        public Guid SocketId { get; } = socketId;
    }

    public class AfterPlayerRequestEventArgs(string token, Protocol.Messages.Message response) : EventArgs
    {
        public string Token { get; } = token;
        public Protocol.Messages.Message Response { get; } = response;
    }

    public event EventHandler<AfterPlayerConnectEventArgs> AfterPlayerConnectEvent = delegate { };
    public event EventHandler<AfterPlayerRequestEventArgs> AfterPlayerRequestEvent = delegate { };
}
