using BrainstormSessions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace BrainstormSessions.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        [HttpGet]
        public string GetLogReport()
        {
            var logCounts = LoggerManager.GetLogsCount();
            var errorLogs = LoggerManager.GetErrorLogs();

            var result = @$"
Info logs count: {logCounts[LogLevel.Information]};
Warn logs count: {logCounts[LogLevel.Warning]};
Debug logs count: {logCounts[LogLevel.Debug]};
Error logs count: {logCounts[LogLevel.Error]};
Fatal logs count: {logCounts[LogLevel.Critical]};

Error logs:
{(errorLogs.Count() > 0 ? errorLogs.Aggregate((cur, next) => $"{cur}\r\n{next}") : "No error logs")}
";

            return result;
        } 
    }
}
