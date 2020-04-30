using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecco.Web.Migrations
{
    public partial class UpdatedCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Cards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
