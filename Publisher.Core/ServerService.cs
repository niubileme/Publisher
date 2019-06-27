using Publisher.Core.Server;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core
{
    public class ServerService
    {
        private static readonly object _obj = new object();
        private static ServerService _instance = null;

        private NetServer _server;
        private ServerSettings _setting;

        private ServerService(ServerSettings settings)
        {
            _setting = settings;

            _server = new NetServer();
            _server.Setup(2012);
            _server.NewSessionConnected += NetServer_NewSessionConnected;
            _server.NewRequestReceived += NetServer_NewRequestReceived;

        }

        public static ServerService Initialize(ServerSettings settings = null)
        {
            if (_instance == null)
            {
                lock (_obj)
                {
                    if (_instance == null)
                    {
                        _instance = new ServerService(settings ?? new ServerSettings());
                    }
                }
            }
            return _instance;
        }

        public bool Start()
        {
           return _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
        }


        private void NetServer_NewRequestReceived(NetSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine("收到新消息");
        }

        private void NetServer_NewSessionConnected(NetSession session)
        {
            Console.WriteLine("新用户");
        }
    }
}
