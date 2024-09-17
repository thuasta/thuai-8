using Serilog;

namespace Thuai.Server.Utility;

/// <summary>
/// A collection of utility functions.
/// </summary>
public static class Tools
{
    /// <summary>
    /// A class for handling logs.
    /// </summary>
    public static class LogHandler
    {
        /// <summary>
        /// Initializes logger configuration.
        /// </summary>
        public static void Initialize()
        {
            LoggerConfiguration logConfig = new();

            logConfig.WriteTo.Console();
            logConfig.MinimumLevel.Information();

            Log.Logger = logConfig.CreateLogger();
        }

        /// <summary>
        /// Creates a logger for a specific component.
        /// </summary>
        /// <param name="component">Name of the component.</param>
        /// <returns>The logger.</returns>
        public static ILogger CreateLogger(string component)
        {
            return Log.Logger.ForContext("Component", component);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="logger">The logger that logs the exception.</param>
        /// <param name="e">The exception.</param>
        public static void LogException(ILogger logger, Exception e)
        {
            logger.Error($"An exception occurred: {e.Message}");
            logger.Debug(e.StackTrace ?? "No stack trace available.");
        }
    }

    public static class MessageHandler
    {
        // TODO: Implement
    }

    /// <summary>
    /// A class for parsing command line arguments.
    /// </summary>
    public static class ArgParser
    {
        /// <summary>
        /// Parses command line arguments.
        /// </summary>
        /// <param name="args">Argunemts from command line.</param>
        /// <returns>Corresponding settings.</returns>
        public static ArgSettings ParseArgs(string[] args)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }

    public static class ConfigLoader
    {
        //TODO: Implement
    }

    /// <summary>
    /// A class for managing tasks.
    /// </summary>
    public static class TaskManager
    {
        private static int _taskId = 0;                 // The ID of the task.

        /// <summary>
        /// Creates a task.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task CreateTask(Action action)
        {
            _taskId++;

            ILogger logger = LogHandler.CreateLogger($"Task {_taskId}");
            logger.Debug("Task created.");

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
