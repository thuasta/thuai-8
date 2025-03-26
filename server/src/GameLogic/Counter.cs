namespace Thuai.Server.GameLogic;

public class Counter(int maxCount)
{
    public int CurrentCount { get; private set; } = 0;
    public int MaxCount { get; init; } = maxCount;

    public bool IsZero => CurrentCount == 0;
    public bool IsFull => CurrentCount == MaxCount;

    /// <summary>
    /// Decrease the count by 1.
    /// </summary>
    public void Decrease()
    {
        if (CurrentCount > 0)
        {
            CurrentCount--;
        }
    }

    /// <summary>
    /// Increase the count by 1.
    /// </summary>
    public void Increase()
    {
        if (CurrentCount < MaxCount)
        {
            CurrentCount++;
        }
    }

    /// <summary>
    /// Reset the count to the maximum.
    /// </summary>
    public void Reset()
    {
        CurrentCount = MaxCount;
    }

    /// <summary>
    /// Clear the count.
    /// </summary>
    public void Clear()
    {
        CurrentCount = 0;
    }
}
