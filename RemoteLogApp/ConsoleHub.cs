using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace RemoteLogApp
{
    public class ConsoleHub : Hub
    {
        public override Task OnDisconnected(bool stopCalled)
        {
            var groupInfo = ConnectionManager.Instance.Find(Context.ConnectionId);
            if (groupInfo != null)
                Groups.Remove(Context.ConnectionId, groupInfo.GroupName);
            return base.OnDisconnected(stopCalled);
        }

        public void Regist(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            ConnectionManager.Instance.Add(Context.ConnectionId, name);
            Groups.Add(Context.ConnectionId, name);
        }

        string GetAppName()
        {
            var app = ConnectionManager.Instance.Find(Context.ConnectionId)?.GroupName;
            if (string.IsNullOrWhiteSpace(app))
            {
                app = Context.ConnectionId;
            }
            return app;
        }

        public void Log(string msg)
        {

            Clients.Group("Manager").Log(GetAppName(), msg);
        }

        public void Info(string msg)
        {
            Clients.Group("Manager").Info(GetAppName(), msg);
        }

        public void Warn(string msg)
        {
            Clients.Group("Manager").Warn(GetAppName(), msg);
        }

        public void Error(string msg)
        {
            Clients.Group("Manager").Error(GetAppName(), msg);
        }

        public void Debug(string msg)
        {
            Clients.Group("Manager").Debug(GetAppName(), msg);
        }
    }
}
