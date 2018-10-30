using CommandLine;
using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using WinServiceBase.Framework.Logging;

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
            var cliArgs = CommandLine.Parser.Default.ParseArguments<Options>( args )
                .WithParsed( opts => RunWithOptions( opts ) );
        }

        private static void RunWithOptions(Options options)
        {
            // Act on the CLI options
            // -c or --console
            if (options.Console)
            {
                ConsoleStartup();
            }
            // If there were no options passed, start the service
            else
            {
                StartWinService();
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
                    Logger.ErrorException(err, string.Format( "An error occurred while starting process {0}", process.ProcessName ) );
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
                Console.WriteLine( "Type 'ExitAll' to stop application: " );

                var input = Console.ReadLine() ?? "";

                // Check if we should exit 1+ processes, exit as appropriate
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
                    var process = processes.Find( p => p.StopCode.ToLower() == input.ToLower());

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
