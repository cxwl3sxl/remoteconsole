using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace RemoteLogApp
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            app.UseController();
        }
    }
}
