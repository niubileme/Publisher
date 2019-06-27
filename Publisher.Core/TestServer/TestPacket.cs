namespace PublisherServer
{

    public class TestPacket
    {
        /// <summary>
        /// 开始符号 6字节, "!Start"
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// 消息类型，1字节
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 主体消息数据包长度，4字节
        /// </summary>
        public uint Lenght { get; set; }

        /// <summary>
        /// 主体消息
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 结束符号4字节, "$End" ,
        /// </summary>
        public string End { get; set; }

        public override string ToString()
        {
            return string.Format("消息类型:{0},主体消息长度:{1}", Type, Lenght);
        }
    }
}
