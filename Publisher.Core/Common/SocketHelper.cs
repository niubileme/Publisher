using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace PublisherCore.Helper
{
    public class SocketHelper
    {
        public static Socket GetSocket()
        {
            uint num = 0u;
            byte[] array = new byte[Marshal.SizeOf(num) * 3];
            BitConverter.GetBytes(1u).CopyTo(array, 0);
            BitConverter.GetBytes(15000u).CopyTo(array, Marshal.SizeOf(num));
            BitConverter.GetBytes(15000u).CopyTo(array, Marshal.SizeOf(num) * 2);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.IOControl(IOControlCode.KeepAliveValues, array, null);
            return socket;
        }

        public static bool Connect(Socket socket, IPEndPoint ipe, int timeout = 0)
        {
            if (timeout == 0)
            {
                while (!socket.Connected)
                {
                    try
                    {
                        socket.Connect(ipe);
                    }
                    catch
                    {
                    }
                }
                return true;
            }
            IAsyncResult asyncResult = socket.BeginConnect(ipe, null, null);
            asyncResult.AsyncWaitHandle.WaitOne(timeout * 1000);
            return socket.Connected;
        }

        public static string Receive(Socket socket, int receiveTimeout = 60)
        {
            //socket.ReceiveTimeout = receiveTimeout * 1000;
            List<byte> list = new List<byte>();
            //接收缓冲区
            byte[] buffer = new byte[1024];
            while (true)
            {
                int n;
                if ((n = socket.Receive(buffer)) > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        list.Add(buffer[i]);
                    }
                }
                if (IsComplete(list))
                {
                    break;
                }
                if (n == 0)
                {
                    Console.WriteLine("断开连接");
                    break;
                }
            }
            return Encoding.UTF8.GetString(list.ToArray(), 0, list.Count);
            
        }

        public static int Send(Socket socket, string msg, int timeout = 0)
        {
            socket.SendTimeout = timeout * 1000;
            byte[] bytes = Encoding.UTF8.GetBytes(msg + "#IZK");
            return socket.Send(bytes, bytes.Length, SocketFlags.None);
        }

        public static bool IsComplete(List<byte> data)
        {
            if (data.Count >= 4 && data[data.Count - 4] == 35 && data[data.Count - 3] == 73 && data[data.Count - 2] == 90 && data[data.Count - 1] == 75)
            {
                for (int i = 0; i < 4; i++)
                {
                    data.RemoveAt(data.Count - 1);
                }
                return true;
            }
            return false;
        }

        public static int GetTimeout(long starttime, int timeout)
        {
            return (int)((long)timeout - (DateTime.Now.Ticks - starttime) / 10000L / 1000L);
        }
    }
}
