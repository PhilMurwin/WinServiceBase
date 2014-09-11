using WinServiceBase.Framework.Infrastructure.Logging;
using System;
using System.Threading;

namespace WinServiceBase.Framework
{
    /// <summary>
    /// Base class for creating processes for the service to execute
    /// </summary>
    public abstract class ProcessBase : IProcessBase
    {
        private Thread _processThread;
        private AutoResetEvent _threadExitEvent;
        private ILogger _logger;

        public abstract string ExitCode
        {
            get;
        }

        public abstract string ExitInstructions
        {
            get;
        }

        public abstract bool CanStartProcess
        {
            get;
        }

        /// <summary>
        /// Returns the name of the current process for use in error messages etc...
        /// </summary>
        public string ProcessName
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// Provides access to a logger for the process
        /// </summary>
        public ILogger ProcessLogger
        {
            get { return _logger ?? (_logger = new NLogLogger(GetType().ToString())); }
        }

        /// <summary>
        /// Starting point for the logic of the process
        /// </summary>
        public abstract void ExecuteProcess();

        /// <summary>
        /// Instantiates Thread to call ExecuteProcess on start; also instantiates the exit event for the thread
        /// <para>Any necessary setup prior to the start of thread should be done in an override that calls 
        /// base.Start() at the end</para>
        /// </summary>
        public virtual void Start()
        {
            try
            {
                ProcessLogger.Info(string.Format("*** Starting service [{0}]", ProcessName));

                //Create event that will be used to stop the thread
                _threadExitEvent = new AutoResetEvent(false);

                _processThread = new Thread(ExecuteProcess);
                _processThread.Start();

                ProcessLogger.Info(ProcessName + " has been successfully started.");
            }
            catch (Exception err)
            {
                ProcessLogger.ErrorException( string.Format( "Service [{0}] threw an exception during startup", ProcessName ), err );
            }
        }

        /// <summary>
        /// Base version of stop provides basic implementation to stop any instantiated threads gracefully
        /// and log any errors.
        /// </summary>
        public virtual void Stop()
        {
            var isThreadStopped = false;

            try
            {
                // Stop thread
                if (_threadExitEvent != null && _processThread != null)
                {
                    //Fire the thread exit event
                    if (_threadExitEvent.Set())
                    {
                        isThreadStopped = _processThread.Join(60 * 1000); //wait 60 seconds
                        if (!isThreadStopped)
                        {
                            ProcessLogger.Warn("Warning: " + ProcessName + " thread has failed to stop in a timely manner.");
                        }
                    }
                    else
                    {
                        ProcessLogger.Warn("Warning: unable to stop thread " + ProcessName + ".");
                    }
                }

                //Close all threads as long as we're stopped.  (don't want to close events that are still in a wait)
                if (isThreadStopped)
                {
                    ProcessLogger.Info(ProcessName + " process has been successfully stopped.");

                    if (_threadExitEvent != null)
                    {
                        _threadExitEvent.Close();
                    }
                }

                //Reset thread components
                _threadExitEvent = null;
                _processThread = null;

                ProcessLogger.Info( string.Format("*** Stop service [{0}]", ProcessName) );
            }
            catch (Exception err)
            {
                ProcessLogger.ErrorException("Exception caught during process (" + ProcessName + ") stop: ", err);
            }
        }

        /// <summary>
        /// Waits sleepInMilliseconds for the exit event to be signaled before continuing
        /// </summary>
        /// <param name="sleepInMilliseconds">Number of milliseconds to wait before continuing on with processing</param>
        /// <returns>True if the exit event has been signaled</returns>
        public bool WaitForExitEvent(int sleepInMilliseconds)
        {
            var eventIndex = WaitHandle.WaitAny(new[] { _threadExitEvent }, (sleepInMilliseconds), false);

            return eventIndex == 0;
        }
    }
}
