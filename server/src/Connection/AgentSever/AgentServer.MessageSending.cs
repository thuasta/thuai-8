using System.Collections.Concurrent;

namespace Thuai.Server.Connection;

public partial class AgentServer
{
    public const int MESSAGE_SENDING_INTERVAL = 10;

    private readonly ConcurrentDictionary<Guid, ConcurrentQueue<Protocol.Messages.Message>> _socketMessageSendingQueue = new();
    private readonly ConcurrentDictionary<Guid, Task> _tasksForSendingMessage = new();
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _ctsForSendingMessage = new();

    public void Publish(Protocol.Messages.Message message, string? token = null)
    {
        try
        {
            foreach (Guid connectionId in _sockets.Keys)
            {
                try
                {
                    if (token is null || (_socketTokens.TryGetValue(connectionId, out string? val) && val == token))
                    {
                        if (_socketMessageSendingQueue.TryGetValue(
                            connectionId, out ConcurrentQueue<Protocol.Messages.Message>? queue
                            ) && queue is not null)
                        {
                            queue.Enqueue(message);
                        }
                        else
                        {
                            _socketMessageSendingQueue.AddOrUpdate(
                                connectionId,
                                new ConcurrentQueue<Protocol.Messages.Message>(),
                                (key, oldValue) => new ConcurrentQueue<Protocol.Messages.Message>()
                            );
                            _socketMessageSendingQueue[connectionId].Enqueue(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to add message to message queue of socket {GetAddress(connectionId)}:");
                    Utility.Tools.LogHandler.LogException(_logger, ex);
                }
            }

            _logger.Debug(
                $"Message \"{message.MessageType}\" published{(
                    token is null ? "" : (" to " + Utility.Tools.LogHandler.Truncate(token, 8))
                )}."
            );
            _logger.Verbose(message.Json);
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to publish message:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }

    private Task CreateTaskForSendingMessage(Guid socketId)
    {
        _logger.Debug($"Creating task for sending message to {GetAddress(socketId)}...");

        CancellationTokenSource cts = new();
        _ctsForSendingMessage.AddOrUpdate(
            socketId,
            cts,
            (key, oldValue) =>
            {
                oldValue?.Cancel();
                return cts;
            }
        );

        return new(() =>
        {
            while (_isRunning)
            {
                if (cts.IsCancellationRequested == true)
                {
                    _logger.Debug($"Request task for sending message to {GetAddress(socketId)} to be cancelled.");
                    return;
                }

                try
                {
                    if (_socketMessageSendingQueue.TryGetValue(socketId, out ConcurrentQueue<Protocol.Messages.Message>? queue))
                    {
                        if (queue.Count > MAXIMUM_MESSAGE_QUEUE_SIZE)
                        {
                            _logger.Warning(
                                $"Message queue for sending to {GetAddress(socketId)} is full. "
                                + "The messages in queue will be cleared."
                            );
                            queue.Clear();
                        }

                        if (queue.TryDequeue(out Protocol.Messages.Message? message) && message is not null)
                        {
                            _sockets[socketId].Send(message.Json);
                            _logger.Debug($"Sent message \"{message.MessageType}\" to {GetAddress(socketId)}.");
                        }
                        else
                        {
                            Task.Delay(MESSAGE_SENDING_INTERVAL).Wait();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to send message to {GetAddress(socketId)}:");
                    Utility.Tools.LogHandler.LogException(_logger, ex);
                }
            }
        }, cts.Token);
    }
}
