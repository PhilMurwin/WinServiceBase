## WinServiceBase Summary
This is a windows service base project that is designed to run multiple processes.  It can be
run from the command line using a flag or as a windows service.  Starts up multiple processes each in it's own
thread.  Processes are expected to have a boolean config flag in the app.config

### Running the application/service
* **To run in Visual Studio Debug mode or from the command line**
    * Running in debug build mode in VS will open via console
    * or Set "-c" in the debug tab's command line arguments
* **To run from a command prompt**
    * pass "-c" to the executable as you would any command line
* **To run as a windows service**
    * Use the "Batch Files" to add the windows service to the local services management console on your local
    or server machine

### Adding a Process
* Create a new class that derives from ProcessBase
    * **Required**: *StopCode*- string variable used to shutdown the process from command line
    * **Required**: *CanStartProcess* - bool variable used to determine if the process should be started when the application is started
    * **Required**: *Frequency* - Sets how often (in minutes) the process should be called
    * **Required**: *DoProcessWork()* - This method is called to execute the process logic.

### Existing Processes
* Basic Time Logger
    * A simple process that creates a log entry in a log file.
    * Primarily used to test that the service is functional

