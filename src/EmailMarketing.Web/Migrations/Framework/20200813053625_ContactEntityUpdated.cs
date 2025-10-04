using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class ContactEntityUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_ContactUploads_ContactUploadId",
                table: "Contacts");

            migrationBuilder.AlterColumn<int>(
                name: "ContactUploadId",
                table: "Contacts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_ContactUploads_ContactUploadId",
                table: "Contacts",
                column: "ContactUploadId",
                principalTable: "ContactUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_ContactUploads_ContactUploadId",
                table: "Contacts");

            migrationBuilder.AlterColumn<int>(
                name: "ContactUploadId",
                table: "Contacts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_ContactUploads_ContactUploadId",
                table: "Contacts",
                column: "ContactUploadId",
                principalTable: "ContactUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
