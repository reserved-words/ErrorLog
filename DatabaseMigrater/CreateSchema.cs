using ErrorLog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseMigrater
{
    public static class CreateSchema
    {
        public static void Run(string connectionString)
        {
            using (var dbContext = new ApplicationDbContext(connectionString))
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
