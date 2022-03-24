using System.Threading.Tasks;
using DotNetty.Codecs.Http;
using PinFun.Core.Net.Http;
using PinFun.Core.ServiceHost.WebHost.Middleware.ExtendWebMiddlewares;
using PinFun.Core.ServiceHost.WebPush;

namespace RemoteConsole
{
    class LogScriptWebMiddleware : IExtendWebMiddleware
    {
        public bool CanProcess(string uri)
        {
            return uri.StartsWith("/log-script");
        }

        public Task Process(HttpContext context)
        {
            var channel = context.Request.QueryString["channel"];
            if (string.IsNullOrEmpty(channel))
            {
                return context.Response.SetHttpResponseStatus(HttpResponseStatus.BadRequest).End();
            }

            var script = Properties
                .Resources
                .log_source
                .Replace("$channel$", channel)
                .Replace("$server$", $"//{context.Request.Headers.Get(HttpHeaderNames.Host, null)}");
            return context.Response.SetContentType("application/json").Write(script).End();
        }

        public string Name => "log脚本";
    }

    class WsWebMiddleware : IExtendWebMiddleware
    {
        private readonly WebPushHostConfig _config;

        public WsWebMiddleware()
        {
            _config = PinFun.Core.Utils.HostConfig.Instance.GetConfig<WebPushHostConfig>("WebPushHost");
        }

        public bool CanProcess(string uri)
        {
            return uri.StartsWith("/ws-config");
        }

        public Task Process(HttpContext context)
        {
            if (_config == null) return context.Response.SetHttpResponseStatus(HttpResponseStatus.BadRequest).End();

            var host = context.Request.Headers.Get(HttpHeaderNames.Host, null)?.ToString();
            if (!string.IsNullOrEmpty(host))
            {
                if (host.Contains(":"))
                {
                    host = host.Split(':')[0];
                }
            }

            var script =
                $"window.pushUrl = '{(string.IsNullOrEmpty(_config.Certificate?.CertificateFile) ? "ws" : "wss")}://{host}:{_config.Port}';";
            return context.Response.SetContentType("application/json").Write(script).End();
        }

        public string Name => "ws配置脚本";
    }
}
