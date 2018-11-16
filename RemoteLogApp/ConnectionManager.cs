using System.Collections.Generic;
using System.Linq;

namespace RemoteLogApp
{
    public class Singletone<T> where T : new()
    {
        public static T Instance { get; private set; }

        static Singletone()
        {
            Instance = new T();
        }
    }

    public class ConnectionInfo
    {
        public ConnectionInfo(string id, string name)
        {
            ConnectionId = id;
            GroupName = name;
        }

        public string ConnectionId { get; }
        public string GroupName { get; }
    }

    public class ConnectionManager : Singletone<ConnectionManager>
    {
        readonly List<ConnectionInfo> _allConnections = new List<ConnectionInfo>();
        readonly object _syncLock = new object();

        public void Add(string id, string name)
        {
            lock (_syncLock)
            {
                var exist = _allConnections.FirstOrDefault(c => c.ConnectionId == id);
                if (exist != null)
                {
                    _allConnections.Remove(exist);
                }
                _allConnections.Add(new ConnectionInfo(id, name));
            }
        }

        public ConnectionInfo Find(string id)
        {
            lock (_syncLock)
            {
                return _allConnections.FirstOrDefault(c => c.ConnectionId == id);
            }
        }
    }
}
