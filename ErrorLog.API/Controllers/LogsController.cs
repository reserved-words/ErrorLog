using System.Collections.Generic;
using System.Linq;
using ErrorLog.Common;
using ErrorLog.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ErrorLog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LogsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Log> Get(string appName)
        {
            using (var dbContext = new ApplicationDbContext(_configuration.GetConnectionString("logs")))
            {
                return dbContext.Logs
                    .Where(lg => string.IsNullOrEmpty(appName) || lg.AppName == appName)
                    .ToList();
            }
        }

        [HttpPost]
        public bool Post([FromBody]Log log)
        {
            using (var dbContext = new ApplicationDbContext(_configuration.GetConnectionString("logs")))
            {
                dbContext.Logs.Add(log);
                dbContext.SaveChanges();
                return true;
            }
        }
    }
}
