using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var appServer = new TestServer();
            appServer.Setup(2012);
            appServer.NewSessionConnected += new SessionHandler<TestSession>(appServer_NewSessionConnected);
            appServer.NewRequestReceived += AppServer_NewRequestReceived;
            appServer.Start();
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }
            appServer.Stop();
        }

        private static void AppServer_NewRequestReceived(TestSession session,StringRequestInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Key);
            Console.WriteLine(requestInfo.Body);
        }

        static void appServer_NewSessionConnected(TestSession session)
        {
            Console.WriteLine("新连接："+session.RemoteEndPoint.ToString());
            //session.Send(session.RemoteEndPoint.ToString());
        }
    }
}
