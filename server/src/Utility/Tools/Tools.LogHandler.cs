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
        /// The maximum length of a log message.
        /// </summary>
        public const int MaximumMessageLength = 65536;

        /// <summary>
        /// The template for Serilog (console).
        /// </summary>
        private const string SerilogTemplate =
            "[{Timestamp:HH:mm:ss.fff} {Level:u3}] " +
            "{Component,-20:default(No Component)} " +
            "{Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// The template for Serilog (file output).
        /// </summary>
        private const string SerilogFileOutputTemplate =
            "[{Timestamp:HH:mm:ss.fff} {Level:u3}] " +
            "{Component,-20:default(No Component)} " +
            "{Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Default log settings.
        /// </summary>
        public static Config.LogSettings DefaultLogSettings => new();

        private static bool _isInitialized = false;

        private static LoggerConfiguration DefaultLogConfig => new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: SerilogTemplate)
            .MinimumLevel.Information();

        private static ILogger? _logger = null;     // Should be created in Initialize()

        /// <summary>
        /// Initializes logger configuration.
        /// </summary>
        public static void Initialize(Config.LogSettings logSettings)
        {
            if (_isInitialized)
            {
                _logger?.Warning("Logger already initialized.");
                return;
            }

            LoggerConfiguration logConfig = new();

            string logFileName = GetLogFileName();
            string logFilePath = Path.Combine(logSettings.TargetDirectory, logFileName);

            bool isValidLogSettings = true;

            switch (logSettings.Target)
            {
                case Config.LogSettings.LogTarget.Console:
                    logConfig.WriteTo.Console(outputTemplate: SerilogTemplate);
                    break;

                case Config.LogSettings.LogTarget.File:
                    logConfig.WriteTo.File(
                        logFilePath,
                        outputTemplate: SerilogFileOutputTemplate,
                        rollingInterval: logSettings.RollingInterval
                    );
                    break;

                case Config.LogSettings.LogTarget.Both:
                    logConfig.WriteTo.Console(outputTemplate: SerilogTemplate);
                    logConfig.WriteTo.File(
                        logFilePath,
                        outputTemplate: SerilogFileOutputTemplate,
                        rollingInterval: logSettings.RollingInterval
                    );
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

            _logger = CreateLogger("LogHandler");

            if (isValidLogSettings == false)
            {
                _logger.Warning("Invalid log settings. Using default settings.");
            }

            _logger.Debug("Initializing Fleck log action...");
            InitializeFleckLogAction();
            _logger.Debug("Fleck log action initialized.");

            _isInitialized = true;

            _logger.Debug("Logger initialized.");
            _logger.Debug($"Log target: {logSettings.Target}");
            _logger.Debug($"Minimum log level: {logSettings.MinimumLevel}");
        }

        /// <summary>
        /// Creates a logger for a specific component.
        /// </summary>
        /// <param name="component">Name of the component.</param>
        /// <returns>The logger.</returns>
        public static ILogger CreateLogger(string componentName)
        {
            return Log.Logger.ForContext("Component", componentName);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="logger">The logger that logs the exception.</param>
        /// <param name="e">The exception.</param>
        public static void LogException(ILogger? logger, Exception e)
        {
            logger?.Error($"An exception occurred: {Truncate(e.Message, MaximumMessageLength)}");
            logger?.Debug(e.StackTrace ?? "No stack trace available.");
        }

        /// <summary>
        /// Truncates a string to a specified length.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="maxLength">Maximum length of the string.</param>
        /// <returns>Truncated string</returns>
        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : string.Concat(value.AsSpan(0, maxLength), "...");
        }

        private static void InitializeFleckLogAction()
        {
            ILogger logger = CreateLogger("Fleck");

            FleckLog.LogAction = (level, message, ex) =>
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        logger.Debug(Truncate(message, MaximumMessageLength));
                        break;
                    case LogLevel.Error:
                        logger.Error(Truncate(message, MaximumMessageLength));
                        break;
                    case LogLevel.Warn:
                        logger.Warning(Truncate(message, MaximumMessageLength));
                        break;
                    default:
                        logger.Information(Truncate(message, MaximumMessageLength));
                        break;
                }

                if (ex != null)
                {
                    LogException(logger, ex);
                }
            };
        }

        private static string GetLogFileName()
        {
            return DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log";
        }
    }
}
