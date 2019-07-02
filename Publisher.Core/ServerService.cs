using Publisher.Core.Common;
using Publisher.Core.Logging;
using Publisher.Core.Server;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher.Core
{
    public class ServerService
    {
        private ILogger _log = LoggerFactory.GetLogger2("ServerService");

        private static readonly object _obj = new object();
        private static ServerService _instance = null;

        private NetServer _server;
        private ServerSettings _setting;

        private ConcurrentDictionary<string, Queue<NetPacket>> _sessionDic;

        private ServerService(ServerSettings settings)
        {
            _setting = settings;
            _sessionDic = new ConcurrentDictionary<string, Queue<NetPacket>>();

            _server = new NetServer();
            _server.Setup(2012);
            _server.NewSessionConnected += Server_NewSessionConnected;
            _server.SessionClosed += Server_SessionClosed;
            _server.NewRequestReceived += Server_NewRequestReceived;

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
            _sessionDic.Clear();
            _server.Stop();
        }


        private void Server_NewSessionConnected(NetSession session)
        {
            _sessionDic.TryAdd(session.SessionID, new Queue<NetPacket>());
        }

        private void Server_SessionClosed(NetSession session, SuperSocket.SocketBase.CloseReason value)
        {
            var queue = new Queue<NetPacket>();
            _sessionDic.TryRemove(session.SessionID, out queue);
        }

        private void Server_NewRequestReceived(NetSession session, NetRequestInfo requestInfo)
        {
            lock (_obj)
            {
                var packet = requestInfo.Packet;

                //var message = new Queue<NetPacket>();
                //if (!session._messages.TryGetValue(session._count, out message))
                //{
                //    Interlocked.Increment(ref session._count);
                //    session._messages.TryAdd(session._count, message);
                //}

                if (packet.PacketIndex == 1)
                {
                    Interlocked.Increment(ref session._count);
                    var message = new Queue<NetPacket>();
                    message.Enqueue(packet);
                    session._messages.TryAdd(session._count, message);
                }
                else
                {
                    var message = new Queue<NetPacket>();
                    if (session._messages.TryGetValue(session._count, out message))
                    {
                        message.Enqueue(packet);
                    }
                }

            }


            //var queue = new Queue<NetPacket>();
            //if (!_sessionDic.TryGetValue(session.SessionID, out queue))
            //    return;


            //if (packet.PacketCount == 1)
            //{
            //    if (packet.PacketIndex != 1)
            //        return;

            //    var type = (int)packet.Type;
            //    switch (type)
            //    {
            //        case 0:
            //            ProcessSingleText(session, packet);
            //            break;
            //        case 1:
            //            ProcessSingleFile(session, packet);
            //            break;
            //        default:
            //            ProcessSingleText(session, packet);
            //            break;
            //    }
            //}
            //else
            //{
            //    queue.Enqueue(packet);
            //    if (packet.PacketIndex == 1)
            //    {
            //        Task.Factory.StartNew(() =>
            //        {
            //            ProcessMultiplePackets(session);
            //        });
            //    }
            //}

        }


        private void ProcessSingleText(NetSession session, NetPacket packet)
        {
            string cmd = "";
            try
            {
                cmd = Encoding.UTF8.GetString(packet.Body);
                var result = CmdHelper.ExecuteCmd(cmd);

                var bytes = Encoding.Default.GetBytes(result);
                session.Send(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                _log.Error("cmd命令:" + cmd, ex);
                session.Send("ExecuteCommand CMD Failed!" + Environment.NewLine + cmd + Environment.NewLine + ex.Message);
            }
        }

        private void ProcessSingleFile(NetSession session, NetPacket packet)
        {

        }

        private void ProcessMultiplePackets(NetSession session)
        {

        }

    }
}
