using System;
using System.Collections.Generic;
using System.ServiceProcess;
using WinServiceBase.Framework;
using WinServiceBase.Framework.Infrastructure.Logging;

namespace WinServiceBase.App_Entry
{
    public partial class WinService : ServiceBase
    {
        private static readonly ILogger _logger = new NLogLogger( "WinService" );

        List<IProcessBase> processes;

        public WinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Get the list of processes to be run
                processes = ProcessSetup.GetProcessList();

                // Start processes
                foreach ( var process in processes )
                {
                    try
                    {
                        process.Start();
                    }
                    catch ( Exception err )
                    {
                        _logger.ErrorException( string.Format( "An error occurred while starting process {0}", process.ProcessName ), err );
                    }
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.WriteLine( string.Format("Startup failed for {0}", this.ServiceName ));
            }
        }

        protected override void OnStop()
        {
            foreach (var process in processes)
            {
                process.Stop();
            }
        }
    }
}
