using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class ContactEntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Groups_GroupId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactUploads_Groups_GroupId",
                table: "ContactUploads");

            migrationBuilder.DropIndex(
                name: "IX_ContactUploads_GroupId",
                table: "ContactUploads");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_GroupId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ContactUploads");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "isProcessing",
                table: "ContactUploads",
                newName: "IsProcessing");

            //migrationBuilder.AddColumn<Guid>(
            //    name: "UserId",
            //    table: "SMTPConfigs",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "FieldMaps",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "Created",
            //    table: "FieldMaps",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<Guid>(
            //    name: "CreatedBy",
            //    table: "FieldMaps",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "LastModified",
            //    table: "FieldMaps",
            //    nullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "LastModifiedBy",
            //    table: "FieldMaps",
            //    nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Contacts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ContactGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ContactId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactGroups_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactUploadGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ContactUploadId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUploadGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactUploadGroups_ContactUploads_ContactUploadId",
                        column: x => x.ContactUploadId,
                        principalTable: "ContactUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactUploadGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactGroups_ContactId",
                table: "ContactGroups",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactGroups_GroupId",
                table: "ContactGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUploadGroups_ContactUploadId",
                table: "ContactUploadGroups",
                column: "ContactUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUploadGroups_GroupId",
                table: "ContactUploadGroups",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactGroups");

            migrationBuilder.DropTable(
                name: "ContactUploadGroups");

            //migrationBuilder.DropColumn(
            //    name: "UserId",
            //    table: "SMTPConfigs");

            //migrationBuilder.DropColumn(
            //    name: "Created",
            //    table: "FieldMaps");

            //migrationBuilder.DropColumn(
            //    name: "CreatedBy",
            //    table: "FieldMaps");

            //migrationBuilder.DropColumn(
            //    name: "LastModified",
            //    table: "FieldMaps");

            //migrationBuilder.DropColumn(
            //    name: "LastModifiedBy",
            //    table: "FieldMaps");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "IsProcessing",
                table: "ContactUploads",
                newName: "isProcessing");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "FieldMaps",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "ContactUploads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContactUploads_GroupId",
                table: "ContactUploads",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_GroupId",
                table: "Contacts",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Groups_GroupId",
                table: "Contacts",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUploads_Groups_GroupId",
                table: "ContactUploads",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
