using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Owin.Hosting;

namespace RemoteLogApp
{
    class WebServer
    {
        private IDisposable _server;

        public static string GetUrl()
        {
            var port = ConfigurationManager.AppSettings["Port"];
            if (string.IsNullOrWhiteSpace(port)) port = "8081";
            return "http://localhost:8083".Replace("8083", port);
        }

        public void Start()
        {
            Url = GetUrl();
            _server = WebApp.Start<Startup>(Url.Replace("localhost", "*"));
            Trace.WriteLine($"[[RemoteLogApp]] 服务已经成功启动，地址为 {Url}");
        }

        public void Stop()
        {
            _server.Dispose();
            Trace.WriteLine("[[RemoteLogApp]] 服务已经成功停止.");
        }

        public string Url { get; private set; } = "";
    }
}
