using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorLog.DataAccess.Migrations
{
    public partial class AddCreateUsersProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Resources.CreateProcedure_CreateUsers);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Resources.DropProcedure_CreateUsers);
        }
    }
}
