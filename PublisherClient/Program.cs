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
                //var cmd = "ipconfig";
                //for (int i = 0; i < 2; i++)
                //{
                //    Console.WriteLine("执行:" + cmd);
                //    var result = client.ExcuteCommand(cmd);
                //    Console.WriteLine(result);

                //}

                var r=client.TransferFile("swiper-4.5.0.zip");
                Console.WriteLine(r);
            }

            Console.WriteLine("over");
            Console.ReadKey();
        }
    }
}
