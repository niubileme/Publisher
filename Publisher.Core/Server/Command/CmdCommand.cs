using Publisher.Core.Common;
using Publisher.Core.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core.Server.Command
{
    /// <summary>
    /// 执行cmd命令
    /// </summary>
    public class CmdCommand : StringCommandBase<NetSession>
    {
        private object _obj = new object();
        private ILogger _log = LoggerFactory.GetLogger2("Commands");

        public override string Name => "CMD";

        public override void ExecuteCommand(NetSession session, StringRequestInfo requestInfo)
        {
            try
            {
                lock (_obj)
                {
                    var result = CmdHelper.ExecuteCmd(requestInfo.Body);

                    var bytes = Encoding.Default.GetBytes(result);
                    session.Send(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error("cmd命令:" + requestInfo.Body, ex);
                session.Send("ExecuteCommand CMD Failed!" + Environment.NewLine + requestInfo.Body + Environment.NewLine + ex.Message);
            }
        }
    }
}
