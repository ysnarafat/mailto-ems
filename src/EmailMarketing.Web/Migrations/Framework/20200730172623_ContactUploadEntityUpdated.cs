using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class ContactUploadEntityUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExistingUpdate",
                table: "ContactUploads");

            migrationBuilder.DropColumn(
                name: "IsSendEmail",
                table: "ContactUploads");

            migrationBuilder.AddColumn<bool>(
                name: "IsSendEmailNotify",
                table: "ContactUploads",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdateExisting",
                table: "ContactUploads",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SendEmailAddress",
                table: "ContactUploads",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSendEmailNotify",
                table: "ContactUploads");

            migrationBuilder.DropColumn(
                name: "IsUpdateExisting",
                table: "ContactUploads");

            migrationBuilder.DropColumn(
                name: "SendEmailAddress",
                table: "ContactUploads");

            migrationBuilder.AddColumn<bool>(
                name: "IsExistingUpdate",
                table: "ContactUploads",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSendEmail",
                table: "ContactUploads",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
