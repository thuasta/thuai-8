namespace Thuai.Server.Connection;

public partial class AgentServer
{
    public class AfterMessageReceiveEventArgs(Message message, Guid socketId) : EventArgs
    {
        public Message Message { get; } = message;
        public Guid SocketId { get; } = socketId;
    }
}
