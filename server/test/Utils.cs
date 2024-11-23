using System.Collections;

namespace Thuai.Server.Test;

/// <summary>
/// Frequently used test data for int type.
/// </summary>
public class TestIntData : IEnumerable<object[]>
{
    public const int RandomTestIterations = 100;
    private static readonly Random _random = new();

    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = 0; i < RandomTestIterations; i++)
        {
            yield return new object[] { _random.Next(int.MinValue, int.MaxValue) };
        }

        yield return new object[] { 0 };
        yield return new object[] { 1 };
        yield return new object[] { -1 };
        yield return new object[] { 100 };
        yield return new object[] { -100 };
        yield return new object[] { int.MaxValue };
        yield return new object[] { int.MinValue };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Frequently used test data for double type.
/// </summary>
public class TestDoubleData : IEnumerable<object[]>
{
    public const int RandomTestIterations = 100;
    private static readonly Random _random = new();

    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = 0; i < RandomTestIterations; i++)
        {
            yield return new object[]
            {
                10.0 * (
                    _random.NextDouble() * (0.1 * double.MaxValue -  0.1 * double.MinValue)
                    + 0.1 * double.MinValue
                )
            };
        }

        yield return new object[] { 0.0 };
        yield return new object[] { 1.0 };
        yield return new object[] { -1.0 };
        yield return new object[] { 100.0 };
        yield return new object[] { -100.0 };
        yield return new object[] { 1e-10 };
        yield return new object[] { -1e-10 };
        yield return new object[] { 1e-20 };
        yield return new object[] { -1e-20 };
        yield return new object[] { 1e-50 };
        yield return new object[] { -1e-50 };
        yield return new object[] { double.MaxValue };
        yield return new object[] { double.MinValue };
        yield return new object[] { double.Epsilon };
        yield return new object[] { double.NaN };
        yield return new object[] { double.PositiveInfinity };
        yield return new object[] { double.NegativeInfinity };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Frequently used test data for string type.
/// </summary>
public class TestStringData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "TestString" };
        yield return new object[] { "aassf s a kfajk o" };
        yield return new object[] { "1234567890" };
        yield return new object[] { "" };
        yield return new object[] { "                     " };
        yield return new object[] { "\t\n\r\a\b\f\v" };
        yield return new object[] { "\x1b[31mTest Action\x1b[0m" };
        yield return new object[] { "\xff\xfeT\xcce\xees\xddt\xa0 \x00t\x00a\x00s\x00k\x00" };
        yield return new object[] { new string('a', 1000000) + new string('b', 1000000) };
        yield return new object[] { "!@#$%^&*()_+-=[]{}|;':\",./<>?" };
        yield return new object[] { "我是汉字" };
        yield return new object[] { "Emoji 😊😂👍" };
        yield return new object[] { "Mixed 123 字符串" };
        yield return new object[] { "Line1\nLine2\nLine3" };
        yield return new object[] { "Null\0Character" };
        yield return new object[] { "Unicode \u263A \u263B" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Frequently used test data for Action type.
/// </summary>
public class TestActionData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new Action(() => { })
        };
        yield return new object[]
        {
            new Action(() => { Task.Delay(100).Wait(); })
        };
        yield return new object[]
        {
            new Action(() => { int result = 1 + 1; result++; })
        };
        yield return new object[]
        {
            new Action(
                () => { int cnt = 0; for (int i = 0; i < 1000; i++) { cnt++; } }
            )
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Frequently used data for action with exception.
/// </summary>
public class TestActionWithExceptionData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new Action(() => throw new InvalidOperationException("Test InvalidOperationException"))
        };
        yield return new object[]
        {
            new Action(() => throw new ArgumentNullException("Test ArgumentNullException"))
        };
        yield return new object[]
        {
            // Divide by zero
            new Action(() => { int a = 1, b = 0; int c =  a / b; c++; })
        };
        yield return new object[]
        {
            // Out of memory
            new Action(() => { List<List<int>> list = []; while(true) { list.Add(new(int.MaxValue)); } })
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class DataGroup<T1, T2> : IEnumerable<object[]>
    where T1 : IEnumerable<object[]>, new()
    where T2 : IEnumerable<object[]>, new()
{
    private IEnumerable<object[]>[] _datas;
    public DataGroup()
    {
        _datas = [new T1(), new T2()];
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (IEnumerable<object[]> data in _datas)
        {
            foreach (object[] item in data)
            {
                yield return item;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class TestParameters<T1, T2> : IEnumerable<object[]>
    where T1 : IEnumerable<object[]>, new()
    where T2 : IEnumerable<object[]>, new()
{
    private readonly IEnumerable<object[]> _param1;
    private readonly IEnumerable<object[]> _param2;

    public TestParameters()
    {
        _param1 = new T1();
        _param2 = new T2();
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (object[] item1 in _param1)
        {
            foreach (object[] item2 in _param2)
            {
                object[] combined = new object[item1.Length + item2.Length];
                item1.CopyTo(combined, 0);
                item2.CopyTo(combined, item1.Length);
                yield return combined;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
