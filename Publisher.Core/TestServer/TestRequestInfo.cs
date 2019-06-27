using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherServer
{
    public class TestRequestInfo : IRequestInfo
    {
        public string Key { get; set; }
        public TestPacket Packet { get; set; }

        public TestRequestInfo(string key, TestPacket packet)
        {
            Key = key;
            Packet = packet;
        }
    }
}
