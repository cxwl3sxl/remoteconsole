using System.Threading.Tasks;
using PinFun.Core.Net.Http;
using PinFun.Core.ServiceHost.WebHost.Middleware.ExtendWebMiddlewares;

namespace RemoteConsole;

class WsConfigMiddleware : IExtendWebMiddleware
{
    private readonly string _wsUrl = PinFun.Core.Utils.HostConfig.Instance.GetConfig<string>("WsUrl");

    public async Task Process(HttpContext context)
    {
        var script = $"window.pushUrl = '{_wsUrl}';";
        await context.Response.SetContentType("application/json").Write(script);
        await context.Response.End();
    }

    public string Description => "ws配置脚本";

    public string InterceptUrl => "/ws-config";
}