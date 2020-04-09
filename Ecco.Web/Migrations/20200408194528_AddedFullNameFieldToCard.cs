using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecco.Web.Migrations
{
    public partial class AddedFullNameFieldToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Cards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Cards");
        }
    }
}
