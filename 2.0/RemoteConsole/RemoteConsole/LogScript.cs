using System.Threading.Tasks;
using DotNetty.Codecs.Http;
using PinFun.Core.Net.Http;
using PinFun.Core.ServiceHost.WebHost.Middleware.ExtendWebMiddlewares;

namespace RemoteConsole
{
    class LogScriptMiddleware : IExtendWebMiddleware
    {
        public async Task Process(HttpContext context)
        {
            var channel = context.Request.QueryString["channel"];
            if (string.IsNullOrEmpty(channel))
            {
                await context.Response.SetHttpResponseStatus(HttpResponseStatus.BadRequest).End();
                return;
            }

            var script = Properties
                .Resources
                .log_source
                .Replace("$channel$", channel)
                .Replace("$server$", $"//{context.Request.Headers.Get(HttpHeaderNames.Host, null)}");
            await context.Response.SetContentType("application/json").Write(script);
            await context.Response.End();
        }

        public string Description => "log脚本";

        public string InterceptUrl => "/log-script";
    }
}
