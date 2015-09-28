using System;
using log4net;

namespace DataCollectorFramework.Logger
{
    public interface ILogger
    {
        void Debug(object message);
        void Info(object message);
        void InfoFormat(string format, params object[] args);
        void WarnFormat(string format, params object[] args);
        void Warn(object message);
        void Warn(object message, Exception exception);
        void Error(object message);
        void Error(object message, Exception exception);
    }

    public class Logger : ILogger
    {
        private readonly ILog _logger;

        public Logger(Type type)
        {
            _logger = LogManager.GetLogger(type);
        }

        public void Debug(object message)
        {
            _logger.Debug(message);
        }

        public void Info(object message)
        {
            _logger.Info(message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _logger.InfoFormat(format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _logger.WarnFormat(format, args);
        }

        public void Warn(object message)
        {
            _logger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _logger.Error(message, exception);
        }
    }
}
