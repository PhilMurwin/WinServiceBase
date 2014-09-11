using NLog;

namespace WinServiceBase.Framework.Infrastructure.Logging
{
    /// <summary>
    /// Logging wrapper for NLog that implements the GMS ILogger Interface
    /// </summary>
    public class NLogLogger : ILogger
    {
        private readonly Logger _logger;

        /// <summary>
        /// NLog wrapper constructor, requires a logger instance name in order to differentiate the various loggers used throughout the system
        /// <para>Generally expected to be called by a Dependency Injection library</para>
        /// </summary>
        /// <param name="loggerInstanceName">Name of the logger instance to be configured
        /// <para>If you rename loggerInstanceName be sure to update the NinjectControllerFactory string as well</para></param>
        public NLogLogger(string loggerInstanceName)
        {
            //_logger = LogManager.GetCurrentClassLogger();
            _logger = LogManager.GetLogger( loggerInstanceName );
        }

        /// <summary>
        /// Creates a log entry
        /// <para>This is typically not expected to be called directly, extension methods simply the calling process</para>
        /// </summary>
        /// <param name="logEntry">The details necessary to create a log entry</param>
        public void Log( LogEntry logEntry )
        {
            var wrapperType = logEntry.WrapperType ?? typeof(NLogLogger);
            var logLevel = GetNLogLevel( logEntry.LogType );

            //if ( logEntry.Exception == null )
            //{
            //    _logger.Log( wrapperType, LogEventInfo.Create( logLevel, _logger.Name, logEntry.Message) );
            //}
            //else
            {
                _logger.Log( wrapperType, LogEventInfo.Create( logLevel, _logger.Name, logEntry.Message, logEntry.Exception ) );
            }
        }

        /// <summary>
        /// Converts a LoggingEventType level to an NLog level type
        /// <para>Defaults to Info if it cannot figure it out</para>
        /// </summary>
        /// <param name="logType">The GMS version of the log level to be mapped to an NLog mapping</param>
        /// <returns>Returns the matching NLog Level</returns>
        private static LogLevel GetNLogLevel(LoggingEventType logType)
        {
            switch (logType)
            {
                case LoggingEventType.Trace:
                    return LogLevel.Trace;
                case LoggingEventType.Debug:
                    return LogLevel.Debug;
                case LoggingEventType.Info:
                    return LogLevel.Info;
                case LoggingEventType.Warn:
                    return LogLevel.Warn;
                case LoggingEventType.Error:
                    return LogLevel.Error;
                case LoggingEventType.Fatal:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Info;
            }
        }
    }
}