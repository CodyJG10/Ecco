using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecco.Web.Migrations
{
    public partial class UpdatedCompanyTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "EmployeeInvitations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Companies",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmployeeInvitations");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Companies");
        }
    }
}
