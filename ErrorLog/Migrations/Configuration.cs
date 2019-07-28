namespace ErrorLog.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ErrorLog.Data;
    using ErrorLog.Data.Enums;

    internal sealed class Configuration : DbMigrationsConfiguration<LogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ErrorLog.Data.LogContext";
        }

        protected override void Seed(LogContext ctx)
        {
            if (!ctx.Apps.Any())
            {
                ctx.Apps.AddOrUpdate(x => x.Id,
                  new App()
                  {
                      Id = 1,
                      Name = "Stronger",
                      Moniker = "stronger",
                      Logs = new List<Log>
                      {
                          new Log
                          {
                              Id = 1,
                              Timestamp = DateTime.Now,
                              Level = Level.Info,
                              Message = "Test log",
                              StackTrace = "N/A"
                          }
                      }
                  });
            }
        }
    }
}
