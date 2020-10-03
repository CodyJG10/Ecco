using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecco.Web.Migrations
{
    public partial class AddedWebsiteAndAddressToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Cards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Cards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Cards");
        }
    }
}
