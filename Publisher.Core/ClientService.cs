using Publisher.Core.Logging;
using PublisherCore.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core
{
    public class ClientService
    {
        private ILogger _log = LoggerFactory.GetLogger2("ClientService");

        private static readonly object _obj = new object();
        private static ClientService _instance = null;

        public IPEndPoint RemoteAddress { get; private set; }
        public bool Connected { get; private set; }

        private Socket _socket;

        private ClientService(IPEndPoint ipe)
        {
            _socket = SocketHelper.GetSocket();
            Connected = SocketHelper.Connect(_socket, ipe);
        }

        public static ClientService Connect(string ip, int port)
        {
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                return Connect(new IPEndPoint(address, port));
            }
            return null;
        }

        public static ClientService Connect(IPEndPoint ipe)
        {
            if (_instance == null)
            {
                lock (_obj)
                {
                    if (_instance == null)
                    {
                        _instance = new ClientService(ipe);
                    }
                }
            }
            return _instance;
        }


        public string ExcuteCommand(string command)
        {
            try
            {
                byte[] body = Encoding.UTF8.GetBytes(command);
                SendPacket(1, 1, body);

                var result = SocketHelper.Receive(_socket);
                return result;
            }
            catch (Exception ex)
            {
                _log.Error("ExcuteCommand", ex);
                return ex.Message;
            }
        }


        public string TransferFile(string path)
        {

            return "";
        }


        private int SendPacket(ushort packetIndex, ushort packetCount, byte[] body, bool isFile = false)
        {
            uint lenght = (uint)body.Length;
            Encoding encoding = Encoding.UTF8;

            List<byte> senddata = new List<byte>();
            senddata.AddRange(encoding.GetBytes("!Start"));//Start
            if (isFile)
                senddata.Add(1);//Type
            else
                senddata.Add(0);//Type
            senddata.AddRange(BitConverter.GetBytes(lenght));//Lenght
            senddata.AddRange(BitConverter.GetBytes(packetIndex));//PacketIndex
            senddata.AddRange(BitConverter.GetBytes(packetCount));//PacketCount
            senddata.Add(0);//Flag
            senddata.AddRange(body);//Body
            senddata.AddRange(encoding.GetBytes("$End"));//End

            return _socket.Send(senddata.ToArray());
        }
    }
}
