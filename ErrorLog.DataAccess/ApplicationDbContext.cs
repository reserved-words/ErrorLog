using ErrorLog.Common;
using Microsoft.EntityFrameworkCore;
using PostDeploymentTools;

namespace ErrorLog.DataAccess
{
    public class ApplicationDbContext : MigratableDbContext
    {
        private const string DefaultConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=apps;Integrated Security=True;";
        private const string SchemaName = "ErrorLog";

        public ApplicationDbContext()
            : this(DefaultConnectionString, SchemaName)
        {
        }

        public ApplicationDbContext(string connectionString, string schemaName)
            :base(connectionString, schemaName)
        {
        }

        public virtual DbSet<Log> Logs { get; set; }
    }
}
