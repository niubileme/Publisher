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

        private static void appServer_NewRequestReceived(TestSession session, TestRequestInfo requestInfo)
        {
            try
            {
                var packet = requestInfo.Packet;
                var type = Convert.ToInt32(packet.Type);
                if (type == 0)
                {
                    var command = Encoding.UTF8.GetString(packet.Body);
                    Console.WriteLine("收到命令：" + command);
                }
                else
                {
                    using (FileStream fs = new FileStream("1.zip", FileMode.Create, FileAccess.Read))
                    {
                        fs.Write(packet.Body, 0, (int)packet.Lenght);
                    }
                    Console.WriteLine("保存文件成功！");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void appServer_NewSessionConnected(TestSession session)
        {
            Console.WriteLine("新连接：" + session.RemoteEndPoint.ToString());
            session.Send(session.RemoteEndPoint.ToString() + " Welcome!");
        }
    }
}
