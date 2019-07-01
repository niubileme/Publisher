using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core.Server
{
    public class NetReceiveFilterFactory : IReceiveFilterFactory<NetRequestInfo>
    {
        public IReceiveFilter<NetRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new NetReceiveFilter();
        }
    }
}
