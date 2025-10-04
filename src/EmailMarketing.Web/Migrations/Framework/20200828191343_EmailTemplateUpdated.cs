using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class EmailTemplateUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPersonalized",
                table: "EmailTemplates",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPersonalized",
                table: "EmailTemplates");
        }
    }
}
