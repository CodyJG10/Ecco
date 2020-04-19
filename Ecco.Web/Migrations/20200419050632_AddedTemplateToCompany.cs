using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecco.Web.Migrations
{
    public partial class AddedTemplateToCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyTemplates");

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "Companies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsingCompanyTemplate",
                table: "Cards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsUsingCompanyTemplate",
                table: "Cards");

            migrationBuilder.CreateTable(
                name: "CompanyTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTemplates", x => x.Id);
                });
        }
    }
}
