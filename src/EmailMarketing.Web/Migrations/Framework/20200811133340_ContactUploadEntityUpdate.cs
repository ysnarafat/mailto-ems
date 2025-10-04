using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class ContactUploadEntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ContactUploads",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ContactUploads");
        }
    }
}
