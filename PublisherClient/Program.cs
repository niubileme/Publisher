using PublisherCore.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PublisherClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var socket = SocketHelper.GetSocket();
            if (SocketHelper.Connect(socket, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012)))
            {

                var bytes = Encoding.Default.GetBytes("#adadada");
                socket.Send(bytes);
            }
            else
            {
                Console.WriteLine("error");
            }

            Console.ReadKey();
        }
    }
}
