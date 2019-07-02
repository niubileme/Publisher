using Publisher.Core.Common;
using Publisher.Core.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher.Core.Server
{
    public class NetSession : AppSession<NetSession, NetRequestInfo>
    {
        public Encoding _encoding { get { return Encoding.UTF8; } }

        private ILogger _log = LoggerFactory.GetLogger2("NetSession");

        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;
        private Task _task;

        private int _index;
        public int _count;
        public ConcurrentDictionary<int, Queue<NetPacket>> _messages;

        public NetSession()
        {
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _messages = new ConcurrentDictionary<int, Queue<NetPacket>>();
        }

        /// <summary>
        /// 用户连接会话
        /// </summary>
        protected override void OnSessionStarted()
        {
            Console.WriteLine("新用户连接");
            _task = Task.Run(ProcessRequest, _token);
            _task.Start();

            base.OnSessionStarted();
        }

        /// <summary>
        /// 会话关闭
        /// </summary>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine("用户已断开:" + reason.ToString());
            _tokenSource.Cancel();
            base.OnSessionClosed(reason);
        }


        /// <summary>
        /// 未知的用户请求命令
        /// </summary>
        /// <param name="requestInfo"></param>
        protected override void HandleUnknownRequest(NetRequestInfo requestInfo)
        {
            Console.WriteLine("未知请求:" + requestInfo.ToString());
            base.HandleUnknownRequest(requestInfo);
        }

        private void ProcessRequest()
        {
            _index = 1;
            while (true)
            {
                if (_token.IsCancellationRequested)
                    return;

                if (_index <= _count)
                {
                    var packets = new Queue<NetPacket>();
                    if (_messages.TryGetValue(_index, out packets))
                    {
                        ProcessPackets(packets);
                        _index++;
                    }
                }
                Thread.Sleep(500);
            }
        }

        private void ProcessPackets(Queue<NetPacket> packets)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    //第一个数据包
                    var packet = packets.Dequeue();
                    var index = packet.PacketIndex;
                    var count = packet.PacketCount;
                    var type = (int)packet.Type;

                    //校验
                    if (index != 1 || count < 1)
                        return;

                    if (count == 1)
                    {
                        //一个数据包 cmd命令
                        if (type != 0)
                        {
                            this.Send("不支持的命令");
                            return;
                        }
                        ExcuteCMD(packet);
                    }
                    else
                    {
                        //多个数据包 文件上传
                        if (type != 1)
                        {
                            this.Send("不支持的命令");
                            return;
                        }
                        //第一个包为文件信息
                        var fileInfo = _encoding.GetString(packet.Body);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("process packets failed! " + ex.Message);
                    this.Send("");
                }
                finally
                {

                }
            });
        }


        private void ExcuteCMD(NetPacket packet)
        {
            string cmd = "";
            try
            {
                cmd = _encoding.GetString(packet.Body);
                var result = CmdHelper.ExecuteCmd(cmd);

                var bytes = Encoding.Default.GetBytes(result);
                this.Send(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                _log.Error("cmd命令:" + cmd, ex);
                this.Send("ExecuteCommand CMD Failed!" + Environment.NewLine + cmd + Environment.NewLine + ex.Message);
            }
        }

        private void Save(NetPacket packet)
        {
            string cmd = "";
            try
            {
                cmd = Encoding.UTF8.GetString(packet.Body);
                var result = CmdHelper.ExecuteCmd(cmd);

                var bytes = Encoding.Default.GetBytes(result);
                this.Send(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                _log.Error("cmd命令:" + cmd, ex);
                this.Send("ExecuteCommand CMD Failed!" + Environment.NewLine + cmd + Environment.NewLine + ex.Message);
            }
        }
    }
}
