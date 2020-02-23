using ErrorLog.Common;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace ErrorLog.DataAccess
{
    public class LogsService : ILogger, ILogAccess
    {
        private readonly IConfiguration _configuration;

        public LogsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Log> GetLogs(string appName)
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.Logs
                    .Where(lg => string.IsNullOrEmpty(appName) || lg.AppName == appName)
                    .ToList();
            }
        }

        public void Log(Log log)
        {
            using (var dbContext = GetDbContext())
            {
                dbContext.Logs.Add(log);
                dbContext.SaveChanges();
            }
        }
        private ApplicationDbContext GetDbContext()
        {
            return new ApplicationDbContext(_configuration.GetConnectionString("Logs"), _configuration["SchemaName"]);
        }
    }
}
