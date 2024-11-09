using System.Collections;

namespace Thuai.Server.Test;

/// <summary>
/// Frequently used test data for int type.
/// </summary>
public class TestIntData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
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
    public IEnumerator<object[]> GetEnumerator()
    {
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
