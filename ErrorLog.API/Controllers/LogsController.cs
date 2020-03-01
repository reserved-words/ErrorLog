using System;
using System.Collections.Generic;
using ErrorLog.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErrorLog.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IBackupLogger _backupLogger;
        private readonly ILogAccess _logAccess;
        private readonly ILogger _logger;

        public LogsController(ILogger logger, IBackupLogger backupLogger, ILogAccess logAccess)
        {
            _backupLogger = backupLogger;
            _logAccess = logAccess;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Log> Get(string appName)
        {
            return _logAccess.GetLogs(appName);
        }

        [HttpPost]
        public IActionResult Post([FromBody]LogContent logContent)
        {
            var log = new Log
            {
                Timestamp = DateTime.Now,
                AppName = logContent.AppName,
                Message = logContent.Message
            };

            try
            {
                _logger.Log(log);
                return Ok("Logged to primary log");
            }
            catch (Exception ex)
            {
                _backupLogger.Log(log);
                _backupLogger.Log(new Log
                {
                    Timestamp = DateTime.Now,
                    Message = $"Previous error logged to backup logger due to exception: {ex.Message}",
                    AppName = "ErrorLog"
                });
                return Ok("Logged to backup log");
            }
        }
    }
}
