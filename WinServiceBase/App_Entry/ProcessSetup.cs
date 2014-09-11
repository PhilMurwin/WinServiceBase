using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WinServiceBase.Framework;

namespace WinServiceBase.App_Entry
{
    static class ProcessSetup
    {
        internal static List<IProcessBase> GetProcessList()
        {
            //Dictionary of processes
            var processes = new List<IProcessBase>();

            processes.AddRange(Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof (ProcessBase))).Select(t => (IProcessBase) Activator.CreateInstance(t)).Where(process => process.CanStartProcess));

            return processes;
        }
    }
}