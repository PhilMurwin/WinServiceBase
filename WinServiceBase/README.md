## WinServiceBase Summary
This is a windows service base project that is designed to run multiple processes semi-dynamically.  It can be
run from the command line using a flag or as a windows service.  Starts up multiple processes each in it's own
thread.  Processes are expected to have a boolean config flag in the app.config

### Running the application/service
* **To run in Visual Studio Debug mode or from the command line**
    * Set "-c" in the debug tab's command line arguments
* **To run from a command prompt**
    * pass "-c" to the executable as you would any command line
* **To run as a windows service**
    * Use the "Batch Files" to add the windows service to the local services management console on your local
    or server machine

### Adding a Process
* Create a new class that derives from ProcessBase
    * REQUIRED: CanStartProcess - bool variable used to determine if the process should be started when the application is started
    * REQUIRED: ExecuteProcess() - This method is called to start the process in it's own thread.  This should
    contain a while loop that exits based on a wait event. (see windows event logger for example)
    * REQUIRED: ExitCode - string variable used to shutdown the process on command
	* REQUIRED: ExitInstructions - string variable printed in the console on startup when the application is started in console mode    

### Existing Processes
* Windows Event Logger
    * A simple process that creates an entry in the windows event log once a minute.
    * Primarily used to test that the service is functional, not used outside the dev environment
