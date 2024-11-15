using System.Collections;

namespace Thuai.Server.Test.UnitTest;

public class TaskManagerTests
{
    private readonly StringWriter _consoleOutput = new();

    public TaskManagerTests()
    {
        Utility.Tools.LogHandler.Initialize(
            new Utility.Config.LogSettings()
            {
                Target = Utility.Config.LogSettings.LogTarget.Console,
                MinimumLevel = Utility.Config.LogSettings.LogLevel.Verbose,
            }
        );
        Console.SetOut(_consoleOutput);
    }

    private class TaskManagerTestDataNormal : IEnumerable<object[]>
    {
        readonly TestActionData _testActionData = new();
        readonly TestStringData _testStringData = new();

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (object[] actionData in _testActionData)
            {
                foreach (object[] stringData in _testStringData)
                {
                    yield return new object[] { actionData[0], stringData[0] };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private class TaskManagerTestDataWithException : IEnumerable<object[]>
    {
        readonly TestActionWithExceptionData _testActionData = new();
        readonly TestStringData _testStringData = new();

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (object[] actionData in _testActionData)
            {
                foreach (object[] stringData in _testStringData)
                {
                    yield return new object[] { actionData[0], stringData[0] };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(TaskManagerTestDataNormal))]
    [ClassData(typeof(TaskManagerTestDataWithException))]
    public void CreateTask_ShouldCreateTask(Action action, string description)
    {
        // Act
        Task task = Utility.Tools.TaskManager.CreateTask(action, description);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(TaskStatus.Created, task.Status);

        var consoleOutput = _consoleOutput.ToString();
        Assert.Contains("Task created", consoleOutput);
        if (description != "")
        {
            if (description.Length > 256)
            {
                Assert.Contains(description[..256], consoleOutput);
            }
            else
            {
                Assert.Contains(description, consoleOutput);
            }
        }
    }

    [Theory]
    [ClassData(typeof(TestActionData))]
    public void CreateTask_ShouldExecuteAction(Action action)
    {
        // Arrange
        bool actionCompleted = false;
        Action expectedAction = () =>
        {
            action();
            actionCompleted = true;
        };
        string description = "Test task";

        // Act
        Task task = Utility.Tools.TaskManager.CreateTask(expectedAction, description);
        task.Start();
        task.Wait();

        // Assert
        Assert.True(actionCompleted);
    }

    [Theory]
    [ClassData(typeof(TestActionWithExceptionData))]
    public void CreateTask_ShouldLogException(Action action)
    {
        // Arrange
        string description = "Test task";

        // Act
        Task task = Utility.Tools.TaskManager.CreateTask(action, description);
        task.Start();
        try
        {
            task.Wait();
        }
        catch (AggregateException)
        {
            // Ignore the exception
        }

        // Assert
        var consoleOutput = _consoleOutput.ToString();
        Assert.Contains("Task crashed", consoleOutput);
    }
}
