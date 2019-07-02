using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core.Server
{
    public class NetSession : AppSession<NetSession, NetRequestInfo>
    {
        public int _count;
        public ConcurrentDictionary<int, Queue<NetPacket>> _messages;

        public NetSession()
        {
            _messages = new ConcurrentDictionary<int, Queue<NetPacket>>();
        }

        /// <summary>
        /// 用户连接会话
        /// </summary>
        protected override void OnSessionStarted()
        {
            Console.WriteLine("新用户连接");
            base.OnSessionStarted();
        }

        /// <summary>
        /// 未知的用户请求命令
        /// </summary>
        /// <param name="requestInfo"></param>
        protected override void HandleUnknownRequest(NetRequestInfo requestInfo)
        {
            Console.WriteLine("未知的用户请求命令 " + requestInfo.ToString());
            base.HandleUnknownRequest(requestInfo);
        }

        /// <summary>
        /// 会话关闭
        /// </summary>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine("用户已断开:" + reason.ToString());
            base.OnSessionClosed(reason);
        }

    }
}
