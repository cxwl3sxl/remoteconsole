using PinFun.Core.Api.Attributes;

namespace RemoteConsole
{
    [Api]
    public interface ILog
    {
        void Send(string category, string level, string msg, string time);
    }
}
