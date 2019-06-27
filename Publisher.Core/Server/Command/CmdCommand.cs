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
    public class CmdCommand : StringCommandBase
    {
        public override string Name => "CMD";

        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            throw new NotImplementedException();
        }
    }
}
