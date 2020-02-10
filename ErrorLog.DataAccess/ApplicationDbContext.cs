using ErrorLog.Common;
using Microsoft.EntityFrameworkCore;

namespace ErrorLog.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        private const string DefaultConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=apps;Integrated Security=True;";

        private readonly string _connectionString;

        public ApplicationDbContext()
            : this(DefaultConnectionString)
        {
        }

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, x => x.MigrationsHistoryTable("__MigrationsHistory"));
        }

        public virtual DbSet<Log> Logs { get; set; }
    }
}
