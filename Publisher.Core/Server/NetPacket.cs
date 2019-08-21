using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core.Server
{
    public class NetPacket
    {
        /// <summary>
        /// 开始符号 6字节  "!Start"
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// 消息类型 1字节  
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 主体消息长度，4字节
        /// </summary>
        public uint Lenght { get; set; }

        /// <summary>
        /// 数据包索引，2字节
        /// </summary>
        public ushort PacketIndex { get; set; }

        /// <summary>
        /// 数据包总数，2字节
        /// </summary>
        public ushort PacketCount { get; set; }

        /// <summary>
        /// 预留字段flag 1字节 
        /// </summary>
        public byte Flag { get; set; }

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
            return string.Format("消息类型:{0},主体消息长度:{1},数据包索引:{2},数据包总数:{3},Flag:{4}", Type, Lenght, PacketIndex, PacketCount, Flag);
        }
    }
}
