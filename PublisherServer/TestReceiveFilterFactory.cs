using System.Net;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace PublisherServer
{
    public class TestReceiveFilterFactory : IReceiveFilterFactory<TestRequestInfo>
    {
        public IReceiveFilter<TestRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new TestReceiveFilter();
        }
    }
}
