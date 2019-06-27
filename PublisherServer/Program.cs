using Publisher.Core;
using System;
using System.IO;
using System.Text;

namespace PublisherServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = ServerService.Initialize();

            if (!server.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            server.Stop();

            Console.WriteLine("The server was stopped!");

        }
    }
}
