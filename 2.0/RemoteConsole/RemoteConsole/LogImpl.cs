using System;
using PinFun.Core.ServiceHost.WebPush;

namespace RemoteConsole
{
    public class LogImpl : ILog
    {
        public void Send(string category, string level, string msg, string time)
        {
            Console.WriteLine($"[{DateTime.Now}] [{category}] [{level}]: {msg}");
            WebEvent.Instance.Raise(new LogRequest()
            {
                Level = level,
                Message = msg,
                Time = time
            },
                conn => string.Equals(category, conn.Session["category"]?.ToString(),
                    StringComparison.OrdinalIgnoreCase));
        }
    }
}
