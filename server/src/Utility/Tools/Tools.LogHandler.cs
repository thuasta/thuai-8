using Fleck;
using Serilog;

namespace Thuai.Server.Utility;

/// <summary>
/// A collection of utility functions.
/// </summary>
public static partial class Tools
{
    /// <summary>
    /// A class for handling logs.
    /// </summary>
    public static class LogHandler
    {
        /// <summary>
        /// The template for Serilog (console).
        /// </summary>
        const string SerilogTemplate =
            "[{Timestamp:HH:mm:ss.fff} {Level:u3}] " +
            "{Component,-16:default(No Component)} " +
            "{Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// The template for Serilog (file output).
        /// </summary>
        const string SerilogFileOutputTemplate =
            "[{Timestamp:HH:mm:ss.fff} {Level:u3}] " +
            "{Component,-16:default(No Component)} " +
            "{Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Default log settings.
        /// </summary>
        public static Config.LogSettings DefaultLogSettings => new()
        {
            Target = Config.LogSettings.LogTarget.Console,
            MinimumLevel = Config.LogSettings.LogLevel.Information,
        };

        static LoggerConfiguration DefaultLogConfig => new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: SerilogTemplate)
            .MinimumLevel.Information();

        private static readonly ILogger _logger = CreateLogger("LogHandler");

        /// <summary>
        /// Initializes logger configuration.
        /// </summary>
        public static void Initialize(Config.LogSettings logSettings)
        {
            LoggerConfiguration logConfig = new();

            bool isValidLogSettings = true;

            switch (logSettings.Target)
            {
                case Config.LogSettings.LogTarget.Console:
                    logConfig.WriteTo.Console(outputTemplate: SerilogTemplate);
                    break;
                case Config.LogSettings.LogTarget.File:
                    logConfig.WriteTo.File("log.txt", outputTemplate: SerilogFileOutputTemplate);
                    break;
                case Config.LogSettings.LogTarget.Both:
                    logConfig.WriteTo.Console(outputTemplate: SerilogTemplate);
                    logConfig.WriteTo.File("log.txt", outputTemplate: SerilogFileOutputTemplate);
                    break;
                default:
                    isValidLogSettings = false;
                    break;
            }

            switch (logSettings.MinimumLevel)
            {
                case Config.LogSettings.LogLevel.Verbose:
                    logConfig.MinimumLevel.Verbose();
                    break;
                case Config.LogSettings.LogLevel.Debug:
                    logConfig.MinimumLevel.Debug();
                    break;
                case Config.LogSettings.LogLevel.Information:
                    logConfig.MinimumLevel.Information();
                    break;
                case Config.LogSettings.LogLevel.Warning:
                    logConfig.MinimumLevel.Warning();
                    break;
                case Config.LogSettings.LogLevel.Error:
                    logConfig.MinimumLevel.Error();
                    break;
                case Config.LogSettings.LogLevel.Fatal:
                    logConfig.MinimumLevel.Fatal();
                    break;
                default:
                    isValidLogSettings = false;
                    break;
            }

            if (isValidLogSettings == false)
            {
                logConfig = DefaultLogConfig;
            }

            Log.Logger = logConfig.CreateLogger();

            if (isValidLogSettings == false)
            {
                _logger.Warning("Invalid log settings. Using default settings.");
            }

            _logger.Debug("Initializing Fleck log action...");
            InitializeFleckLogAction();
            _logger.Debug("Fleck log action initialized.");

            _logger.Debug("Logger initialized.");
            _logger.Debug($"Log target: {logSettings.Target}");
            _logger.Debug($"Minimum log level: {logSettings.MinimumLevel}");
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

        private static void InitializeFleckLogAction()
        {
            ILogger logger = CreateLogger("Fleck");

            FleckLog.LogAction = (level, message, ex) =>
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        logger.Debug(message);
                        break;
                    case LogLevel.Error:
                        logger.Error(message);
                        break;
                    case LogLevel.Warn:
                        logger.Warning(message);
                        break;
                    default:
                        logger.Information(message);
                        break;
                }

                if (ex != null)
                {
                    LogException(logger, ex);
                }
            };
        }
    }
}
