﻿using CommandLine;

namespace WinServiceBase.App
{
    class Options
    {
        [Option( 'c', "console", Default = false, HelpText = "Start the windows service application in console (command line) mode." )]
        public bool Console { get; set; }
    }
}
