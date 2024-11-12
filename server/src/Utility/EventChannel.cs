using System.Collections.Concurrent;
using Serilog;

namespace Thuai.Server.Utility;

public static class EventChannel
{
    private static readonly ConcurrentDictionary<
        Type, ConcurrentDictionary<Action<object?, EventArgs>, object?>
    > _eventHandlers = new();

    private static ILogger? _logger = null;    // Should be created after LogHandler.Initialize

    /// <summary>
    /// Initializes the event channel.
    /// </summary>
    public static void Initialize()
    {
        _logger = Tools.LogHandler.CreateLogger("EventChannel");

        _logger.Debug("Event channel initialized.");
    }

    /// <summary>
    /// Registers an event handler to the event channel.
    /// </summary>
    /// <param name="eventHandler">The event handler.</param>
    /// <param name="eventTypes">Event types handled by the event handler.</param>
    public static void Register(Action<object?, EventArgs> eventHandler, params Type[] eventTypes)
    {
        try
        {
            foreach (Type eventType in eventTypes)
            {
                var handlers = _eventHandlers.GetOrAdd(
                    eventType, _ => new ConcurrentDictionary<Action<object?, EventArgs>, object?>()
                );
                handlers[eventHandler] = null;

                _logger?.Debug(
                    $"A new event handler registered for event type {eventType.FullName}"
                );
            }
        }
        catch (Exception e)
        {
            Tools.LogHandler.LogException(_logger, e);
        }
    }

    /// <summary>
    /// Unregisters an event handler from the event channel.
    /// </summary>
    /// <param name="eventHandler">The event handler.</param>
    /// <param name="eventTypes">Event types handled by the event handler.</param>
    public static void Unregister(Action<object?, EventArgs> eventHandler, params Type[] eventTypes)
    {
        try
        {
            foreach (Type eventType in eventTypes)
            {
                if (_eventHandlers.TryGetValue(eventType, out var handlers))
                {
                    if (handlers.TryRemove(eventHandler, out _))
                    {
                        _logger?.Debug($"A handler is removed for event type {eventType.Name}");
                    }
                    else
                    {
                        _logger?.Warning($"Failed to remove a handler for event type {eventType.Name}");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Tools.LogHandler.LogException(_logger, e);
        }
    }

    /// <summary>
    /// Add target event to the collection to automatically collect the event.
    /// </summary>
    /// <param name="onTargetEvent">Event to be collected.</param>
    public static void AddToCollection(ref EventHandler onTargetEvent)
    {
        if (!ContainsHandler(ref onTargetEvent, Collect))
        {
            onTargetEvent += Collect;
        }
    }

    /// <summary>
    /// Remove target event from the collection.
    /// </summary>
    /// <param name="onTargetEvent">Event to be removed from collection.</param>
    public static void RemoveFromCollection(ref EventHandler? onTargetEvent)
    {
        if (onTargetEvent is not null && ContainsHandler(ref onTargetEvent, Collect))
        {
            onTargetEvent -= Collect;
        }
    }

    /// <summary>
    /// Collects the event and then distributes it to all registered event handlers.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">Event args.</param>
    public static void Collect(object? sender, EventArgs e)
    {
        try
        {
            _logger?.Debug($"Event collected: {e.GetType().FullName}");

            if (_eventHandlers.TryGetValue(e.GetType(), out var handlers))
            {
                foreach (var eventHandler in handlers.Keys)
                {
                    eventHandler(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            Tools.LogHandler.LogException(_logger, ex);
        }
    }

    /// <summary>
    /// Checks if the target event contains the handler to check.
    /// </summary>
    /// <param name="targetEvent">The target event.</param>
    /// <param name="handlerToCheck">Handler to be checked.</param>
    /// <returns></returns>
    private static bool ContainsHandler(ref EventHandler targetEvent, EventHandler handlerToCheck)
    {
        if (targetEvent == null)
        {
            return false;
        }

        foreach (Delegate existingHandler in targetEvent.GetInvocationList())
        {
            if (existingHandler == (Delegate)handlerToCheck)
            {
                return true;
            }
        }

        return false;
    }
}
