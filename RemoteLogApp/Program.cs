using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;

namespace RemoteLogApp
{
    static class Program
    {
        private const string ServiceName = "RemoteConsoleSvr";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (args == null || args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else if (args[0].ToLower() == "install")
            {
                try
                {
                    ServiceHelper.Install(
                        ServiceName,
                        "浏览器远程日志服务",
                        $@"""{Assembly.GetExecutingAssembly().Location}"" start",
                        $"用于浏览器的远程Console查看服务{Environment.NewLine}可以通过浏览器打开{WebServer.GetUrl()}进行日志查看",
                        ServiceStartType.Auto);
                    Trace.WriteLine($"服务{ServiceName}已经安装成功！");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"服务安装失败：{ex.Message}");
                }
            }
            else if (args[0].ToLower() == "uninstall")
            {
                try
                {
                    ServiceHelper.Uninstall(ServiceName);
                    Trace.WriteLine($"服务{ServiceName}已经卸载成功！");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"服务卸载失败：{ex.Message}");
                }
            }
            else if (args[0].ToLower() == "start")
            {
                try
                {
                    var servicesToRun = new ServiceBase[] { new WinService() };
                    ServiceBase.Run(servicesToRun);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"服务启动失败：{ex.Message}");
                }
            }
            else
            {
                Trace.WriteLine($"Not support command {args[0]}");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject?.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = args.Name.Split(',')[0];
            Debug.WriteLine("正在加载 " + name);
            switch (name)
            {
                case "Microsoft.AspNet.SignalR.Core":
                    return Assembly.Load(Properties.Resources.Microsoft_AspNet_SignalR_Core);
                case "Microsoft.AspNet.SignalR.Owin":
                    return Assembly.Load(Properties.Resources.Microsoft_AspNet_SignalR_Owin);
                case "Microsoft.Owin.Cors":
                    return Assembly.Load(Properties.Resources.Microsoft_Owin_Cors);
                case "Microsoft.Owin.Diagnostics":
                    return Assembly.Load(Properties.Resources.Microsoft_Owin_Diagnostics);
                case "Microsoft.Owin":
                    return Assembly.Load(Properties.Resources.Microsoft_Owin);
                case "Microsoft.Owin.FileSystems":
                    return Assembly.Load(Properties.Resources.Microsoft_Owin_FileSystems);
                case "Microsoft.Owin.Host.HttpListener":
                    return Assembly.Load(Properties.Resources.Microsoft_Owin_Host_HttpListener);
                case "Microsoft.Owin.Hosting":
                    return Assembly.Load(Properties.Resources.Microsoft_Owin_Hosting);
                case "Microsoft.Owin.Security":
                    return Assembly.Load(Properties.Resources.Microsoft_Owin_Security);
                case "Microsoft.Owin.StaticFiles":
                    return Assembly.Load(Properties.Resources.Microsoft_Owin_StaticFiles);
                case "Newtonsoft.Json":
                    return Assembly.Load(Properties.Resources.Newtonsoft_Json);
                case "Owin":
                    return Assembly.Load(Properties.Resources.Owin);
                case "System.Web.Cors":
                    return Assembly.Load(Properties.Resources.System_Web_Cors);
            }
            return null;
        }
    }
}
