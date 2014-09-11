
namespace WinServiceBase.Framework
{
    public struct ProcessDetails
    {
        public string ProcessName { get; set; }
        public string ProcessExitInstructions { get; set; }
        public IProcessBase Process { get; set; }
    }
}
