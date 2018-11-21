using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace RemoteLogApp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject?.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = args.Name.Split(',')[0];
            Debug.WriteLine(name);
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
