namespace Thuai.Server.Connection;

public partial class AgentServer
{
    public class AfterMessageReceiveEventArgs(Protocol.Messages.Message message, Guid socketId) : EventArgs
    {
        public Protocol.Messages.Message Message { get; } = message;
        public Guid SocketId { get; } = socketId;
    }
}
