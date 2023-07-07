using WinServiceBase.Framework.Logging;
using System;
using System.Threading;

namespace WinServiceBase.Framework
{
    /// <summary>
    /// Base class for creating processes for the service to execute
    /// </summary>
    public abstract class ProcessBase : IProcessBase
    {
        protected const int DefaultSleepSeconds = 30 * 1000;
        protected const int MillisecondsInMinute = 60 * 1000;

        private Thread _processThread;
        private AutoResetEvent _threadExitEvent;
        private ILogger _logger;
        private int DelayStartupTime = 2 * MillisecondsInMinute;

        /// <summary>
        /// Provides access to a logger for the process
        /// </summary>
        public ILogger ProcessLogger
        {
            get { return _logger ?? ( _logger = new NLogLogger( GetType().ToString() ) ); }
        }

        /// <summary>
        /// Returns the name of the current process for use in error messages etc...
        /// </summary>
        public string ProcessName
        {
            get { return GetType().Name; }
        }

        public abstract string StopCode
        {
            get;
        }

        public string ExitInstructions
        {
            get { return $"[{ProcessName}] process started. Type '{StopCode}' to stop the process."; }
        }

        public abstract bool CanStartProcess
        {
            get;
        }

        public virtual bool DelayStartup { get; } = false;

        private DateTime m_LastRunTime = DateTime.Now;

        /// <summary>
        /// This time is used to determine how often (the frequency) DoProcessWork is executed.
        /// <para>It defaults to the current time on initialization.</para>
        /// </summary>
        protected DateTime LastRunTime
        {
            get
            {
                return m_LastRunTime;
            }
            set
            {
                m_LastRunTime = value;
            }
        }

        /// <summary>
        /// Frequncy (in minutes) to execute the process.
        /// <para>This should typically be set via a config setting that is read in the override of this property.</para>
        /// </summary>
        public abstract int Frequency
        {
            get;
        }

        /// <summary>
        /// Default Process Loop
        /// <para>Sleeps the thread/process for some amount of time after each run of DoProcessWork.</para>
        /// <para>Override this if you need a less frequent process run frequency.</para>
        /// <para>If you override this method don't call the base version and be sure to set the LastRunTime after your process executes</para>
        /// </summary>
        public virtual void ExecuteProcess()
        {
            // Set LastRunTime to now - frequency so the process executes immediately on startup
            LastRunTime = DateTime.Now.AddMinutes( -Frequency );

            if (DelayStartup)
            {
                Thread.Sleep(DelayStartupTime);
            }

            ProcessLogger.Trace($"Running ExecuteProcess for [{ProcessName}]");

            while ( true )
            {
                if (DateTime.Now >= LastRunTime.AddMinutes(Frequency ) )
                {
                    ProcessLogger.Debug( $"Calling DoProcessWork for [{ProcessName}].");

                    DoProcessWork();

                    LastRunTime = DateTime.Now;
                }

                // Wait n milliseconds for exit event signal before continuing
                // If exit event signaled break out of loop
                if (WaitForExitEvent(DefaultSleepSeconds ) )
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Process Logic
        /// </summary>
        public abstract void DoProcessWork();

        /// <summary>
        /// Instantiates Thread to call ExecuteProcess on start; also instantiates the exit event for the thread
        /// <para>Any necessary setup prior to the start of thread should be done in an override that calls 
        /// base.Start() at the end</para>
        /// </summary>
        public virtual void Start()
        {
            try
            {
                ProcessLogger.Trace($"Start process [{ProcessName}]");

                //Create event that will be used to stop the thread
                _threadExitEvent = new AutoResetEvent(false);

                _processThread = new Thread(ExecuteProcess);
                _processThread.Start();

                ProcessLogger.Info($"[{ProcessName}] has been started.");
            }
            catch (Exception err)
            {
                ProcessLogger.ErrorException( err, $"Service [{ProcessName}] threw an exception during startup" );
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
                            ProcessLogger.Warn($"Warning: [{ProcessName}] thread has failed to stop in a timely manner.");
                        }
                    }
                    else
                    {
                        ProcessLogger.Warn($"Warning: unable to stop thread [{ProcessName}].");
                    }
                }

                //Close all threads as long as we're stopped.  (don't want to close events that are still in a wait)
                if (isThreadStopped)
                {
                    ProcessLogger.Debug( $"[{ProcessName}] process has been successfully stopped.");

                    if (_threadExitEvent != null)
                    {
                        _threadExitEvent.Close();
                    }
                }

                //Reset thread components
                _threadExitEvent = null;
                _processThread = null;

                ProcessLogger.Info( $"Stop process [{ProcessName}]" );
            }
            catch (Exception err)
            {
                ProcessLogger.ErrorException(err, $"Exception caught while stopping process [{ProcessName}]." );
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
