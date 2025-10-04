using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class ContactUploadEntityIsProcessingdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessing",
                table: "ContactUploads",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessing",
                table: "ContactUploads");
        }
    }
}
