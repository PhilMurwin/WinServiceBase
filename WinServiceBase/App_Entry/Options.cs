﻿using CommandLine;

namespace WinServiceBase.App_Entry
{
    class Options
    {
        [Option( 'c', "console", DefaultValue = false, HelpText = "Start the windows service application in console (command line) mode." )]
        public bool Console { get; set; }
    }
}
