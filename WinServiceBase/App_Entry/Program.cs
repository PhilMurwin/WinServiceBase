using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using WinServiceBase.Framework.Infrastructure.Logging;

namespace WinServiceBase.App_Entry
{
    static class Program
    {
        [DllImport( "kernel32.dll" )]
        public static extern Boolean AllocConsole();

        private static readonly ILogger Logger = new NLogLogger( "Program" );

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main( string[] args )
        {
            // Parse command line options
            var options = new Options();
            if ( CommandLine.Parser.Default.ParseArguments( args, options ) )
            {
                // Act on the CLI options
                if ( options.Console || ( args.Length > 0 && args[0].ToLower() == "/console" ) )
                {
                    ConsoleStartup();
                }
                // If there were no options passed start the service
                else
                {
                    StartWinService();
                }
            }
        }

        private static void ConsoleStartup()
        {
            Logger.Info( "*** Running Service in command line mode" );

            //Requests a console window from the OS and attaches it to our process
            AllocConsole();

            // Get the list of processes to be run
            var processes = ProcessSetup.GetProcessList();

            // Start processes
            foreach ( var process in processes )
            {
                try
                {
                    process.Start();
                }
                catch ( Exception err )
                {
                    Logger.ErrorException( string.Format( "An error occurred while starting process {0}", process.ProcessName ), err );
                }
            }

            //Wait for the user to exit the application
            bool exitLoop = false;

            while ( !exitLoop )
            {
                foreach ( var process in processes )
                {
                    Console.WriteLine( process.ExitInstructions );
                }
                Console.WriteLine( "Enter ExitAll to stop application: " );

                var input = Console.ReadLine() ?? "";

                if (input.ToLower() == "exitall")
                {
                    exitLoop = true;
                    foreach (var process in processes)
                    {
                        process.Stop();
                    }
                }
                else
                {
                    var process = processes.Find( p => p.ExitCode.ToLower() == input.ToLower());
                    if (process != null)
                    {
                        process.Stop();
                        processes.Remove(process);
                        exitLoop = processes.Count <= 0;
                    }
                }
            }
        }

        private static void StartWinService()
        {
            ServiceBase[] servicesToRun = new ServiceBase[] { new WinService() };
            ServiceBase.Run( servicesToRun );
        }
    }
}
