using System;
using PinFun.Core.ServiceHost.WebPush;

namespace RemoteConsole
{
    public class LogImpl : ILog
    {
        public void Send(string category, string level, string msg, string time)
        {
            WebEvent.Instance.Raise(new LogRequest()
            {
                Level = level,
                Message = msg,
                Time = time
            },
                conn =>
                {
                    Console.WriteLine("category=" + conn.Session["category"]);
                    return string.Equals(category, conn.Session["category"]?.ToString(),
                        StringComparison.OrdinalIgnoreCase);
                });
        }
    }
}
