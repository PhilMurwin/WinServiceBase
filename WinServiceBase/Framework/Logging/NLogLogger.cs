using System;
using System.Globalization;
using NLog;

namespace WinServiceBase.Framework.Logging
{
    /// <summary>
    /// Logging wrapper for NLog that implements the GMS ILogger Interface
    /// </summary>
    public class NLogLogger : ILogger
    {
        private readonly Type _wrapperType = typeof( NLogLogger );
        private readonly Logger _logger;

        /// <summary>
        /// NLog wrapper constructor, requires a logger instance name in order to differentiate the various loggers used throughout the system
        /// <para>Generally expected to be called by a Dependency Injection library</para>
        /// </summary>
        /// <param name="loggerInstanceName">Name of the logger instance to be configured
        /// <para>If you rename loggerInstanceName be sure to update the NinjectControllerFactory string as well</para></param>
        public NLogLogger( string loggerInstanceName )
        {
            _logger = LogManager.GetLogger( loggerInstanceName );
        }

        /// <summary>
        /// NLog wrapper constructor, requires a type to differentiate the various loggers used throughout the system
        /// <para>Generally expected to be called by a Dependency Injection library</para>
        /// </summary>
        /// <param name="loggerType">Type of the logger instance to be configured, Type.FullName will be used as the logger name</param>
        public NLogLogger( Type loggerType ) : this( loggerType.FullName ) { }


        /// <summary>
        /// Most detailed information. Expect these to be written to logs only. Since version 1.2.12
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Trace( string message, params object[] args )
        {
            Trace( null, null, message, args );
        }

        /// <summary>
        /// Most detailed information. Expect these to be written to logs only. Since version 1.2.12
        /// <para>This should only be used by Web Services where multiple clients may be calling the same service (i.e. Address Verification) </para>
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Trace( string environment, string client, string message, params object[] args )
        {
            LogStandard( environment, client, LogLevel.Trace, message, args );
        }

        /// <summary>
        /// Detailed information on the flow through the system.  Expect these to be written to logs only.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Debug( string message, params object[] args )
        {
            Debug( null, null, message, args );
        }

        /// <summary>
        /// Detailed information on the flow through the system.  Expect these to be written to logs only.
        /// <para>This should only be used by Web Services where multiple clients may be calling the same service (i.e. Address Verification) </para>
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Debug( string environment, string client, string message, params object[] args )
        {
            LogStandard( environment, client, LogLevel.Debug, message, args );
        }

        /// <summary>
        /// Interesting runtime events(startup/shutdown). Expect these to be immediately visible on a console, so be conservative and keep to a minimum.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Info( string message, params object[] args )
        {
            Info( null, null, message, args );
        }

        /// <summary>
        /// Interesting runtime events(startup/shutdown). Expect these to be immediately visible on a console, so be conservative and keep to a minimum.
        /// <para>This should only be used by Web Services where multiple clients may be calling the same service (i.e. Address Verification) </para>
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Info( string environment, string client, string message, params object[] args )
        {
            LogStandard( environment, client, LogLevel.Info, message, args );
        }

        /// <summary>
        /// Use of deprecated APIs, poor use of API, 'almost' errors, other runtime situations that are undesirable or unexpected, but not necessarily "wrong".
        /// <para>Expect these to be immediately visible on a status console.</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warn( string message, params object[] args )
        {
            Warn( null, null, message, args );
        }

        /// <summary>
        /// Use of deprecated APIs, poor use of API, 'almost' errors, other runtime situations that are undesirable or unexpected, but not necessarily "wrong".
        /// <para>Expect these to be immediately visible on a status console.</para>
        /// <para>This should only be used by Web Services where multiple clients may be calling the same service (i.e. Address Verification) </para>
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warn( string environment, string client, string message, params object[] args )
        {
            LogStandard( environment, client, LogLevel.Warn, message, args );
        }

        /// <summary>
        /// Runtime errors or unexpected conditions. Expect these to be immediately visible on a status console.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Error( string message, params object[] args )
        {
            Error( null, null, message, args );
        }

        /// <summary>
        /// Runtime errors or unexpected conditions. Expect these to be immediately visible on a status console.
        /// <para>This should only be used by Web Services where multiple clients may be calling the same service (i.e. Address Verification) </para>
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Error( string environment, string client, string message, params object[] args )
        {
            LogStandard( environment, client, LogLevel.Error, message, args );
        }

        /// <summary>
        /// Severe errors that cause premature termination. Expect these to be immediately visible on a status console.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Fatal( string message, params object[] args )
        {
            Fatal( null, null, message, args );
        }

        /// <summary>
        /// Severe errors that cause premature termination. Expect these to be immediately visible on a status console.
        /// <para>This should only be used by Web Services where multiple clients may be calling the same service (i.e. Address Verification) </para>
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Fatal( string environment, string client, string message, params object[] args )
        {
            LogStandard( environment, client, LogLevel.Fatal, message, args );
        }

        /// <summary>
        /// Runtime errors or unexpected conditions. Expect these to be immediately visible on a status console and emailed to the dev team.
        /// </summary>
        /// <param name="err"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void ErrorException( Exception err, string message, params object[] args )
        {
            LogException( null, null, LogLevel.Error, err, message, args );
        }

        /// <summary>
        /// Runtime errors or unexpected conditions. Expect these to be immediately visible on a status console and emailed to the dev team.
        /// <para>This should only be used by Web Services where multiple clients may be calling the same service (i.e. Address Verification) </para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void ErrorException( string environment, string client, Exception err, string message, params object[] args )
        {
            LogException( environment, client, LogLevel.Error, err, message, args );
        }

        /// <summary>
        /// Severe errors that cause premature termination. Expect these to be immediately visible on a status console and emailed to the dev team.
        /// </summary>
        /// <param name="err"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void FatalException( Exception err, string message, params object[] args )
        {
            LogException( null, null, LogLevel.Fatal, err, message, args );
        }

        /// <summary>
        /// Severe errors that cause premature termination. Expect these to be immediately visible on a status console and emailed to the dev team.
        /// <para>This should only be used by Web Services where multiple clients may be calling the same service (i.e. Address Verification) </para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="err"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        public void FatalException( string environment, string client, Exception err, string message, params object[] args )
        {
            LogException( environment, client, LogLevel.Fatal, err, message, args );
        }

        /// <summary>
        /// Creates a log entry
        /// </summary>
        private void LogStandard( string environment, string client, LogLevel logLevel, string message, params object[] args )
        {
            var theEvent = LogEventInfo.Create( logLevel, _logger.Name, CultureInfo.InvariantCulture, message, args );
            _logger.Log( _wrapperType, theEvent );
        }

        /// <summary>
        /// Creates a log entry that includes exception details
        /// </summary>
        private void LogException( string environment, string client, LogLevel logLevel, Exception err, string message, params object[] args )
        {
            var theEvent = LogEventInfo.Create( logLevel, _logger.Name, CultureInfo.InvariantCulture, message, args );
            theEvent.Exception = err;

            _logger.Log( _wrapperType, theEvent );
        }
    }
}