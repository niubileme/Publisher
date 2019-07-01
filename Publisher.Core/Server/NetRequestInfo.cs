using SuperSocket.SocketBase.Protocol;

namespace Publisher.Core.Server
{
    public class NetRequestInfo : IRequestInfo
    {
        public string Key { get; set; }
        public NetPacket Packet { get; set; }

        public NetRequestInfo(string key, NetPacket packet)
        {
            Key = key;
            Packet = packet;
        }
    }
}
