using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class TemplateEntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailBody",
                table: "EmailTemplates");

            migrationBuilder.AddColumn<string>(
                name: "EmailTemplateBody",
                table: "EmailTemplates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailTemplateBody",
                table: "EmailTemplates");

            migrationBuilder.AddColumn<string>(
                name: "EmailBody",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
