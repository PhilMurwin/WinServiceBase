using System;

namespace WinServiceBase.Framework.Infrastructure.Logging
{
    public class LogEntry
    {
        public LoggingEventType LogType { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public Type WrapperType { get; set; }

        public LogEntry(LoggingEventType logType, string message, Exception exception)
        {
            LogType = logType;
            Message = message;
            Exception = exception;
        }

        public LogEntry( LoggingEventType logType, string message, Exception exception, Type wrapperType ) : this(logType, message, exception)
        {
            WrapperType = wrapperType;
        }
    }
}