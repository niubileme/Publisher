using log4net;
using System;

namespace Publisher.Core.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(ILog log)
        {
            _log = log;
        }

        public void Debug(string msg, params object[] args)
        {
            _log.DebugFormat(msg, args);
        }

        public void Info(string msg, params object[] args)
        {
            _log.InfoFormat(msg, args);
        }

        public void Warn(string msg, params object[] args)
        {
            _log.WarnFormat(msg, args);
        }

        public void Error(string msg, params object[] args)
        {
            _log.ErrorFormat(msg, args);
        }

        public void Error(string msg, Exception ex)
        {
            _log.Error(msg, ex);
        }

        public void Fatal(string msg, params object[] args)
        {
            _log.FatalFormat(msg, args);
        }

        public void Fatal(string msg, Exception ex)
        {
            _log.Fatal(msg, ex);
        }

    }
}
