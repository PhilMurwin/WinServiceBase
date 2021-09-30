using System;
using WinServiceBase.Framework;

namespace WinServiceBase.Processes
{
    /// <summary>
    /// Creates a log entry once a minute
    /// Useful as a test of the service framework and basic layout of a process
    /// </summary>
    public class BasicTimeLogger : ProcessBase
    {
        public override string StopCode
        {
            get { return "ExitLogger"; }
        }

        public override bool CanStartProcess
        {
            get { return ConfigKeys.BasicTimeLogger; }
        }

        public override int Frequency
        {
            get
            {
                return ConfigKeys.BasicTimeLoggerFrequency;
            }
        }

        /// <summary>
        /// Send message to logger once a minute
        /// </summary>
        public override void DoProcessWork()
        {
            //Write current time to eventlog
            var logMessage = string.Format( "The current time is: {0}.", DateTime.Now.ToString( "HH:mm:ss tt" ) );
            ProcessLogger.Info( logMessage );
        }
    }
}
