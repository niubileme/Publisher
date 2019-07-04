using Publisher.Core;
using PublisherCore.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PublisherClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = ClientService.Connect("127.0.0.1", 2012);
            if (client.Connected)
            {
                var cmd = "ipconfig";
                Console.WriteLine("执行:" + cmd);
                var result = client.ExcuteCommand(cmd);
                Console.WriteLine(result);
            }

            Console.WriteLine("over");
            Console.ReadKey();
        }
    }
}
