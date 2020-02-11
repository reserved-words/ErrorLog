using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DatabaseMigrater
{
    public static class CreateUsers
    {
        public static void Run(string connectionString, string databaseName, string appUser)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@DatabaseName", databaseName),
                    new SqlParameter("@AppUser", appUser)
                };

                using (var command = new SqlCommand($"[dbo].[CreateErrorLogUser]", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
