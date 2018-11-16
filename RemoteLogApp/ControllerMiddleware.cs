using Microsoft.Owin;
using Owin;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RemoteLogApp
{
    public class ControllerMiddleware : OwinMiddleware
    {
        public ControllerMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            var url = context.Request.Path.Value.ToLower();
            switch (url)
            {
                case "/": return context.Response.WriteAsync(Properties.Resources.index);
                case "/scripts/jquery.signalr-2.4.0.min.js": return context.Response.WriteAsync(Properties.Resources.jquery_signalR_2_4_0_min);
                case "/scripts/jquery-1.6.4.min.js": return context.Response.WriteAsync(Properties.Resources.jquery_1_6_4_min);
                case "/favicon.ico":
                    using (var ms = new MemoryStream())
                    {
                        Properties.Resources.console.Save(ms);
                        return context.Response.WriteAsync(ms.ToArray());
                    }
                case "/scripts/client.js": return context.Response.WriteAsync(Properties.Resources.client);
            }
            Trace.WriteLine(url);
            return Task.Delay(0);
        }
    }

    public static class ControllerMiddlewareExtension
    {
        public static IAppBuilder UseController(this IAppBuilder builder)
        {
            return builder.Use<ControllerMiddleware>();
        }
    }
}
