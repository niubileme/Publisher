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

            var socket = SocketHelper.GetSocket();
            if (SocketHelper.Connect(socket, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012)))
            {

                SendCommand(socket, "测试命令asdafafa");
            }
            else
            {
                Console.WriteLine("error");
            }

            Console.ReadKey();
        }

        static void SendCommand(Socket socket, string command)
        {
            Encoding encoding = Encoding.UTF8;
            List<byte> senddata = new List<byte>();
            senddata.AddRange(encoding.GetBytes("!Start"));//Start
            senddata.Add(0);//key
            string value = command;
            byte[] body = encoding.GetBytes(value);
            uint len = (uint)body.Length;

            senddata.AddRange(BitConverter.GetBytes(len));//Lenght
            senddata.AddRange(body);//Body
            senddata.AddRange(encoding.GetBytes("$End"));//End

            socket.Send(senddata.ToArray());
        }

        static void SendFile(Socket socket, string command)
        {
            using (FileStream fsWrite = new FileStream("swiper-4.5.0.zip", FileMode.Create, FileAccess.Write))
            {
                using (FileStream fsRead = new FileStream(@"C:\Users\qing\Desktop\2013-11-23\视频\上午1.avi", FileMode.Open, FileAccess.Read))
                {
                    byte[] bs = new byte[1024 * 1024 * 10];
                    int len = fsRead.Read(bs, 0, bs.Length);
                    while (len > 0)
                    {
                        fsWrite.Write(bs, 0, len);
                        len = fsRead.Read(bs, 0, bs.Length);
                    }
                }
            }
            Encoding encoding = Encoding.UTF8;
            List<byte> senddata = new List<byte>();
            senddata.AddRange(encoding.GetBytes("!Start"));//Start
            senddata.Add(0);//key
            string value = command;
            byte[] body = encoding.GetBytes(value);
            uint len = (uint)body.Length;

            senddata.AddRange(BitConverter.GetBytes(len));//Lenght
            senddata.AddRange(body);//Body
            senddata.AddRange(encoding.GetBytes("$End"));//End

            socket.Send(senddata.ToArray());
        }
    }
}
