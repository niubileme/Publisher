using Publisher.Core.Common;
using Publisher.Core.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
            _task = Task.Run(()=> { ProcessRequest(); }, _token);
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
                        ProcessPackets(_index, packets);
                        _index++;
                    }
                }
                Thread.Sleep(500);
            }
        }

        private void ProcessPackets(int messagesIndex, Queue<NetPacket> packets)
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
                        //一个数据包 只能是文本
                        if (type != 0)
                        {
                            this.SendMessage("不支持的命令");
                            return;
                        }
                        ExecuteCommand(packet);
                    }
                    else
                    {
                        //多个数据包 文件上传
                        if (type != 1)
                        {
                            this.SendMessage("不支持的命令");
                            return;
                        }
                        //第一个包为文件信息
                        var fileInfo = _encoding.GetString(packet.Body);
                        var fileInfoSplid = fileInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (string.IsNullOrWhiteSpace(fileInfo) || fileInfoSplid.Length != 2)
                            return;
                        var fileName = fileInfoSplid[0];
                        var fileOutPut = fileInfoSplid[1];

                        //临时文件
                        var extension = Path.GetExtension(fileName);
                        var tempPath = FileHelper.GetRandomTempFile(".zip");
                        using (FileStream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                        {
                            //是否最后一个包
                            var isEnd = false;
                            while (!isEnd)
                            {
                                if (packets.Count > 0)
                                {
                                    var currentPacket = packets.Dequeue();
                                    var currentIndex = currentPacket.PacketIndex;
                                    if (currentIndex == count)
                                        isEnd = true;

                                    var body = currentPacket.Body;
                                    fs.Write(body, 0, body.Length);
                                }
                                else
                                {
                                    Thread.Sleep(100);
                                }
                            }
                        }

                        this.SendMessage(tempPath);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("ProcessPackets Failed! ",ex);
                    this.SendMessage("ProcessPackets Failed!");
                }
                finally
                {
                    //清理
                    var message = new Queue<NetPacket>();
                    _messages.TryRemove(messagesIndex, out message);
                }
            });
        }


        private void ExecuteCommand(NetPacket packet)
        {
            string cmd = "";
            try
            {
                cmd = _encoding.GetString(packet.Body);
                var result = CmdHelper.ExecuteCmd(cmd);

                this.SendMessage(result);
            }
            catch (Exception ex)
            {
                _log.Error("cmd命令:" + cmd, ex);
                this.SendMessage("ExecuteCommand CMD Failed!" + Environment.NewLine + cmd + Environment.NewLine + ex.Message);
            }
        }


        private void SendMessage(string msg)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(msg + "#IZK");
            this.Send(bytes,0,bytes.Length);
        }

    }
}
