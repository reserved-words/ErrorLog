using ErrorLog.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorLog.Migrations;

namespace ErrorLog.Data
{
    public class LogContext : DbContext
    {
        public LogContext() : base("LogConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LogContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ErrorLog");
        }

        public DbSet<App> Apps { get; set; }
        public DbSet<Log> Logs { get; set; }

    }
}
