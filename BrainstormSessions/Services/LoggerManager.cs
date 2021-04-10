using BrainstormSessions.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace BrainstormSessions.Services
{
    public static class LoggerManager
    {
        private static Dictionary<Type, ILogger> _loggers;

        static LoggerManager()
        {
            _loggers = new Dictionary<Type, ILogger>();
        }

        public static ILogger GetLogger<TModel>()
        {
            if (!_loggers.ContainsKey(typeof(TModel)))
            {
                _loggers.Add(typeof(TModel), new Logger<TModel>());
            }

            return _loggers[typeof(TModel)];
        }
    }
}
