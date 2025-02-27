namespace Thuai.Server.Connection;

/// <summary>
/// Constructor
/// </summary>
/// <param name="message">The message received</param>
public class AfterMessageReceiveEventArgs(Message message, Guid socketId) : EventArgs
{
    /// <summary>
    /// The message received
    /// </summary>
    public Message Message { get; } = message;
    public Guid SocketId { get; } = socketId;
}
