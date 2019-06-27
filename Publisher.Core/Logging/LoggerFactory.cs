using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core.Logging
{
    public class LoggerFactory
    {
        private static readonly object lockRepository = new object();
        private static readonly string loggerRepository;
        private static ConcurrentDictionary<string, ILoggerRepository> _dic = new ConcurrentDictionary<string, ILoggerRepository>();

        static LoggerFactory()
        {
            loggerRepository = "NetCoreRepository";
            var repository = LogManager.CreateRepository(loggerRepository);
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public static ILogger GetLogger(string name = "")
        {
            ILog logger = LogManager.GetLogger(loggerRepository, name);
            return new Log4NetLogger(logger);
        }

        public static ILogger GetLogger2(string repositoryName)
        {
            if (string.IsNullOrWhiteSpace(repositoryName))
                return GetLogger();

            if (!_dic.ContainsKey(repositoryName))
            {
                lock (lockRepository)
                {
                    try
                    {
                        var newRepository = LogManager.CreateRepository(repositoryName);
                        LoadRollingFileAppender(newRepository);
                        _dic.TryAdd(repositoryName, newRepository);
                    }
                    catch { }

                }
            }
            return new Log4NetLogger(LogManager.GetLogger(repositoryName, string.Empty));
        }

        private static void LoadRollingFileAppender(ILoggerRepository repository)
        {
            LoadLayout(GetAppender(repository, "Fatal", Level.Fatal), repository);
            LoadLayout(GetAppender(repository, "Error", Level.Error), repository);
            LoadLayout(GetAppender(repository, "Warn", Level.Warn), repository);
            LoadLayout(GetAppender(repository, "Info", Level.Info), repository);
            LoadLayout(GetAppender(repository, "Debug", Level.Debug), repository);
        }

        private static RollingFileAppender GetAppender(ILoggerRepository repository, string levelName, Level level)
        {
            RollingFileAppender rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.Name = "RollingFile" + levelName + "Appender";
            rollingFileAppender.Threshold = level;
            rollingFileAppender.File = "logs/" + repository.Name + "/";
            rollingFileAppender.Encoding = Encoding.UTF8;
            rollingFileAppender.DatePattern = "yyyy-MM-dd/'" + levelName + ".log'";
            rollingFileAppender.StaticLogFileName = false;
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.MaximumFileSize = "1024kb";
            rollingFileAppender.MaxSizeRollBackups = 10;
            rollingFileAppender.LockingModel = new FileAppender.MinimalLock();
            rollingFileAppender.AddFilter(new LevelRangeFilter
            {
                LevelMin = level,
                LevelMax = level
            });
            return rollingFileAppender;
        }

        private static void LoadLayout(AppenderSkeleton appender, ILoggerRepository repository)
        {
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "记录时间：%date 线程ID:[%thread] 日志级别：%-5level ：%logger %n%message%n%n";
            patternLayout.ActivateOptions();
            appender.Layout = patternLayout;
            appender.ActivateOptions();
            BasicConfigurator.Configure(repository, appender);
        }
    }
}
