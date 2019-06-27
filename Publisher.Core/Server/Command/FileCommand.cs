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
    /// 传输文件
    /// </summary>
    public class FileCommand : StringCommandBase
    {
        public override string Name => "FILE";

        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            throw new NotImplementedException();
        }
    }
}
