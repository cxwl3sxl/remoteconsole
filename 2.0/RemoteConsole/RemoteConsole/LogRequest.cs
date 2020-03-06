using PinFun.Core.ServiceHost.WebPush;

namespace RemoteConsole
{
    public class LogRequest : IWebEvent
    {
        public string Time { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}
