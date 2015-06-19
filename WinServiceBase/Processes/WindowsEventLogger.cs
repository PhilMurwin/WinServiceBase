using System;
using System.Diagnostics;
using System.Threading;
using WinServiceBase.Framework;
using WinServiceBase.Framework.Infrastructure.Logging;

namespace WinServiceBase.Processes
{
    /// <summary>
    /// Creates a log entry in the windows Event Log every minute
    /// Useful as a test of the service framework and basic layout of a process
    /// </summary>
    public class WindowsEventLogger : ProcessBase
    {
        private EventLog _windowsEventLog;

        public override string StopCode
        {
            get { return "ExitLogger"; }
        }

        public override bool CanStartProcess
        {
            get { return ConfigKeys.WindowsEventLogger; }
        }

        /// <summary>
        /// Send message to windows event log once a minute
        /// </summary>
        public override void ExecuteProcess()
        {
            //Loop until the thread gets aborted
            try
            {
                while ( true )
                {
                    //Write current time to eventlog
                    _windowsEventLog.WriteEntry( string.Format( "INFO (WinServiceBase.Execute): Current time is: {0}.", DateTime.Now.ToString( "HH:mm:ss" ) ) );
                    ProcessLogger.Info( "test nlog Logger" );

                    // Wait n milliseconds for exit event signal before continuing
                    // If exit event signaled break out of loop
                    if ( WaitForExitEvent( MillisecondsInMinute ) )
                    {
                        break;
                    }
                }
            }
            catch ( ThreadAbortException err )
            {
                _windowsEventLog.WriteEntry( "ERR (WinServiceBase.Execute): Thread aborted." );
                ProcessLogger.ErrorException( "ERR (WinServiceBase.Execute): Thread aborted.", err );
            }
        }

        /// <summary>
        /// Perform some basic setup before starting the processes thread of execution
        /// </summary>
        public override void Start()
        {
            try
            {
                //Check if the Service Event Log Source exists, 
                // if not, create it.
                if ( !EventLog.SourceExists( "EventLoggerSource" ) )
                {
                    EventLog.CreateEventSource( "EventLoggerSource", "Event Logger" );
                }

                _windowsEventLog = new EventLog( "Event Logger" ) { Source = "WinServiceBase" };

                base.Start();
            }
            catch ( Exception err )
            {
                ProcessLogger.ErrorException( "Windows Event Logger threw an exception during startup", err );
            }
        }
    }
}
