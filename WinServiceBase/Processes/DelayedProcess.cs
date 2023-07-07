using WinServiceBase.App;
using WinServiceBase.Framework;

namespace WinServiceBase.Processes
{
    public class DelayedProcess : ProcessBase
    {
        public override string StopCode => "ExitDelayed";

        public override bool CanStartProcess => ConfigKeys.DelayedProcess;

        public override int Frequency => ConfigKeys.DelayedProcessFrequency;

        public override bool DelayStartup => true;

        public override void DoProcessWork()
        {
            ProcessLogger.Info("Run Delayed Process");
        }
    }
}
