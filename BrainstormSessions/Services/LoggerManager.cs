using BrainstormSessions.Core.Interfaces;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BrainstormSessions.Services
{
    public static class LoggerManager
    {
        private const string REGEX_ERROR_FINDER = @"[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3} \[[0-9]*\] ERROR( )*App( )*\r\n(.+)\r\n \r\n";
        private const string REGEX_WARN_COUNTER = @"[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3} \[[0-9]*\] WARN( )*App( )*\r\n";
        private const string REGEX_INFO_COUNTER = @"[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3} \[[0-9]*\] INFO( )*App( )*\r\n";
        private const string REGEX_DEBUG_COUNTER = @"[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3} \[[0-9]*\] DEBUG( )*App( )*\r\n";
        private const string REGEX_FATAL_COUNTER = @"[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3} \[[0-9]*\] FATAL( )*App( )*\r\n";

        private static Dictionary<Type, Core.Interfaces.ILogger> _loggers;
        private static string _logFilePath;

        static LoggerManager()
        {
            _loggers = new Dictionary<Type, Core.Interfaces.ILogger>();
            var rootAppender = ((Hierarchy)LogManager.GetRepository())
                                         .Root.Appenders.OfType<FileAppender>()
                                         .FirstOrDefault();

            _logFilePath = rootAppender != null ? rootAppender.File : string.Empty;
            var fileNameIndex = _logFilePath.IndexOf($"\\{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
            _logFilePath = _logFilePath.Remove(fileNameIndex);
        }

        public static Core.Interfaces.ILogger GetLogger<TModel>()
        {
            if (!_loggers.ContainsKey(typeof(TModel)))
            {
                _loggers.Add(typeof(TModel), new Logger<TModel>());
            }

            return _loggers[typeof(TModel)];
        }
        
        public static IEnumerable<string> GetErrorLogs()
        {
            if (!Directory.Exists(_logFilePath))
            {
                return null;
            }

            var errorLogsFindRegex = new Regex(REGEX_ERROR_FINDER);

            var result = new List<string>();
            var files = Directory.GetFiles(_logFilePath);
            foreach (var file in files)
            {
                var fileContent = File.ReadAllText(file);

                result.AddRange(errorLogsFindRegex.Matches(fileContent).Select(match => match.Value));
            }

            return result;
        }

        public static Dictionary<LogLevel, int> GetLogsCount()
        {
            if (!Directory.Exists(_logFilePath))
            {
                return null;
            }

            var infoLogsCountRegex = new Regex(REGEX_INFO_COUNTER);
            var debugLogsCountRegex = new Regex(REGEX_DEBUG_COUNTER);
            var warnLogsCountRegex = new Regex(REGEX_WARN_COUNTER);
            var errorLogsCountRegex = new Regex(REGEX_ERROR_FINDER);
            var fatalLogsCountRegex = new Regex(REGEX_FATAL_COUNTER);

            var result = new Dictionary<LogLevel, int>() {
                { LogLevel.Information, 0 },
                { LogLevel.Error, 0},
                { LogLevel.Warning, 0},
                { LogLevel.Critical, 0},
                { LogLevel.Debug, 0}
            };
            var files = Directory.GetFiles(_logFilePath);
            foreach (var file in files)
            {
                var fileContent = File.ReadAllText(file);

                result[LogLevel.Debug] += debugLogsCountRegex.Matches(fileContent).Count;
                result[LogLevel.Information] += infoLogsCountRegex.Matches(fileContent).Count;
                result[LogLevel.Error] += errorLogsCountRegex.Matches(fileContent).Count;
                result[LogLevel.Warning] += warnLogsCountRegex.Matches(fileContent).Count;
                result[LogLevel.Critical] += fatalLogsCountRegex.Matches(fileContent).Count;
            }

            return result;
        }
    }
}
