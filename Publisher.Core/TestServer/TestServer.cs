using SuperSocket.SocketBase;

namespace PublisherServer
{
    public class TestServer : AppServer<TestSession, TestRequestInfo>
    {
        public TestServer() : base(new TestReceiveFilterFactory())
        {
        }
    }
}
