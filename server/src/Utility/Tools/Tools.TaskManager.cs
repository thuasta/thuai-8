using Serilog;

namespace Thuai.Server.Utility;

public static partial class Tools
{

    /// <summary>
    /// A class for managing tasks.
    /// </summary>
    public static class TaskManager
    {
        private static int _taskId = 0;                 // The ID of the task.

        /// <summary>
        /// Creates a task.
        /// </summary>
        /// <param name="action">Action to be executed in the task.</param>
        /// <param name="description">Describes what the task does.</param>
        /// <returns></returns>
        public static Task CreateTask(Action action, string description = "")
        {
            _taskId++;

            ILogger logger = LogHandler.CreateLogger($"Task {_taskId}");
            logger.Debug(
                "Task created." + (description == "" ? "" : $" ({LogHandler.Truncate(description, 256)})")
            );

            return new Task(
                () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        logger.Error($"Task crashed:");
                        LogHandler.LogException(logger, e);
                    }
                }
            );
        }
    }
}
