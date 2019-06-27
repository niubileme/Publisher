using SuperSocket.SocketBase.Protocol;
using System;
using System.Text;

namespace PublisherServer
{
    public class TestReceiveFilter : IReceiveFilter<TestRequestInfo>
    {        
        public int LeftBufferSize { get; }

        public IReceiveFilter<TestRequestInfo> NextReceiveFilter { get; }

        public FilterState State { get; }

        /// <summary>
        /// 数据包解析
        /// </summary>
        /// <param name="readBuffer">接收缓冲区</param>
        /// <param name="offset">接收到的数据在缓冲区的起始位置</param>
        /// <param name="length">本轮接收到的数据长度</param>
        /// <param name="toBeCopied">为接收到的数据重新创建一个备份而不是直接使用接收缓冲区</param>
        /// <param name="rest">接收缓冲区未被解析的数据</param>
        /// <returns></returns>
        public TestRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            //当你在接收缓冲区中找到一条完整的请求时，你必须返回一个你的请求类型的实例.
            //当你在接收缓冲区中没有找到一个完整的请求时, 你需要返回 NULL.
            //当你在接收缓冲区中找到一条完整的请求, 但接收到的数据并不仅仅包含一个请求时，设置剩余数据的长度到输出变量 "rest".SuperSocket 将会检查这个输出参数 "rest", 如果它大于 0, 此 Filter 方法 将会被再次执行, 参数 "offset" 和 "length" 会被调整为合适的值.

            Encoding encoding = Encoding.UTF8;

            rest = 0;
            //6+1+4+4
            if (length <= 15)
                return null;

            byte[] data = new byte[length];
            Buffer.BlockCopy(readBuffer, offset, data, 0, length);

            TestPacket myData = new TestPacket();
            myData.Start = encoding.GetString(data, 0, 6);//开始符号 6字节
            myData.Type = data[6];//消息类型 1字节
            myData.Lenght = BitConverter.ToUInt32(data, 7);//主体消息长度 4字节  6 + 1

            if (length < myData.Lenght + 15)
                return null;

            myData.Body = new byte[myData.Lenght];//主体消息
            Buffer.BlockCopy(data, 11, myData.Body, 0, (int)myData.Lenght);

            myData.End = encoding.GetString(data, (int)(11 + myData.Lenght), 4);//结束符号 4字节

            if (myData.Start != "!Start" || myData.End != "$End")
                return null;

            rest = (int)(length - (15 + myData.Lenght));//未处理数据

            return new TestRequestInfo(myData.Type.ToString(), myData);
        }

        public void Reset()
        {
        }
    }
}
