using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace RemoteLogApp
{
    partial class WinService : ServiceBase
    {
        private readonly WebServer _webServer = new WebServer();
        public WinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _webServer.Start();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[[RemoteLogApp]] 服务启动失败：{ex}");
            }
        }

        protected override void OnStop()
        {
            try
            {
                _webServer.Stop();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[[RemoteLogApp]] 服务停止失败：{ex}");
            }
        }
    }
}
