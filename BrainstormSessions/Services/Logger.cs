using BrainstormSessions.Core.Interfaces;
using log4net;

namespace BrainstormSessions.Services
{
    public class Logger<TModel> : ILogger
    {
        private readonly ILog _log;

        public Logger()
        {
            _log = LogManager.GetLogger(typeof(TModel));
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }
    }
}
