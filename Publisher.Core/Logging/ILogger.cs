using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core.Logging
{
    public interface ILogger
    {
        void Debug(string msg, params object[] args);
        void Info(string msg, params object[] args);
        void Warn(string msg, params object[] args);
        void Error(string msg, params object[] args);
        void Error(string msg, Exception ex);
        void Fatal(string msg, params object[] args);
        void Fatal(string msg, Exception ex);
    }
}
