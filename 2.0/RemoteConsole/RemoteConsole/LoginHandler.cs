using PinFun.Core.DependenceInversion;
using PinFun.Core.Net.Common;
using PinFun.Core.ServiceHost.WebPush;

namespace RemoteConsole
{
    [DepInvImplement(SingleInstance = true, Version = 1)]
    class LoginHandler : IWebSocketClientLoginHandler
    {
        public WsClientLoginResult Login(IConnection connection, string clientInfo)
        {
            connection.Session["category"] = clientInfo;
            return new WsClientLoginResult { Successed = true };
        }
    }
}
