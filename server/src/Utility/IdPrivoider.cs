namespace Thuai.Server.Utility;

public class IdProvider
{
    private int _nextId = 0;
    private readonly object _lock = new();

    public int GetNextId()
    {
        lock (_lock)
        {
            return _nextId++;
        }
    }
}
