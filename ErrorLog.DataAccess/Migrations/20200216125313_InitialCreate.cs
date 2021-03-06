﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorLog.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ErrorLog");

            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "ErrorLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    AppName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs",
                schema: "ErrorLog");
        }
    }
}
