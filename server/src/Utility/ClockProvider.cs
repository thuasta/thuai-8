namespace Thuai.Server.Utility;

public class ClockProvider(int milliseconds)
{
    public int Milliseconds { get; private set; } = milliseconds;

    public Task CreateClock()
    {
        return Task.Delay(Milliseconds);
    }
}
