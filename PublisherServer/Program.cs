using System;
using System.IO;
using System.Text;

namespace PublisherServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var appServer = new TestServer();
            appServer.Setup(2012);
            appServer.NewSessionConnected += appServer_NewSessionConnected;
            appServer.NewRequestReceived += appServer_NewRequestReceived;
            appServer.Start();
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }
            appServer.Stop();
        }
    }
}
