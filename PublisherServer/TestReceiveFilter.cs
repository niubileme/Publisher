using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherServer
{
    public class TestReceiveFilter : IReceiveFilter<TestRequestInfo>
    {
        public int LeftBufferSize => throw new NotImplementedException();

        public IReceiveFilter<TestRequestInfo> NextReceiveFilter => throw new NotImplementedException();

        public FilterState State => throw new NotImplementedException();

        public TestRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
