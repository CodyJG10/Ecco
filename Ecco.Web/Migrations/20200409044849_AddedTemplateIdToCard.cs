using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecco.Web.Migrations
{
    public partial class AddedTemplateIdToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "Cards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "Cards");
        }
    }
}
