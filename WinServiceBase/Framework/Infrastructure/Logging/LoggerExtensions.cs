using System;

namespace WinServiceBase.Framework.Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        private static readonly Type LoggerExtensionType = typeof( LoggerExtensions );

        public static void Trace( this ILogger logger, string message )
        {
            logger.Log( new LogEntry( LoggingEventType.Trace, message, null, LoggerExtensionType ) );
        }

        public static void Debug( this ILogger logger, string message )
        {
            logger.Log( new LogEntry( LoggingEventType.Debug, message, null, LoggerExtensionType ) );
        }

        public static void Info( this ILogger logger, string message )
        {
            logger.Log( new LogEntry( LoggingEventType.Info, message, null, LoggerExtensionType ) );
        }

        public static void Warn( this ILogger logger, string message )
        {
            logger.Log( new LogEntry( LoggingEventType.Warn, message, null, LoggerExtensionType ) );
        }

        public static void Error( this ILogger logger, string message )
        {
            logger.Log( new LogEntry( LoggingEventType.Error, message, null, LoggerExtensionType ) );
        }

        public static void Fatal( this ILogger logger, string message )
        {
            logger.Log( new LogEntry( LoggingEventType.Fatal, message, null, LoggerExtensionType ) );
        }

        public static void ErrorException(this ILogger logger, string message, Exception exception )
        {
            logger.Log( new LogEntry( LoggingEventType.Error, message, exception, LoggerExtensionType ) );
        }
    }
}