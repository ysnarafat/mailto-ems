using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class ContactUploadImportEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SingleValueEntries");

            migrationBuilder.AddColumn<int>(
                name: "ContactUploadId",
                table: "Contacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContactUploads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<Guid>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    IsSucceed = table.Column<bool>(nullable: false),
                    IsExistingUpdate = table.Column<bool>(nullable: false),
                    HasColumnHeader = table.Column<bool>(nullable: false),
                    IsSendEmail = table.Column<bool>(nullable: false),
                    SucceedEntryCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactUploads_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldMaps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    IsStandard = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUploadFieldMaps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    FieldMapId = table.Column<int>(nullable: false),
                    ContactUploadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUploadFieldMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactUploadFieldMaps_ContactUploads_ContactUploadId",
                        column: x => x.ContactUploadId,
                        principalTable: "ContactUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactUploadFieldMaps_FieldMaps_FieldMapId",
                        column: x => x.FieldMapId,
                        principalTable: "FieldMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactValueMaps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: false),
                    FieldMapId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactValueMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactValueMaps_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactValueMaps_FieldMaps_FieldMapId",
                        column: x => x.FieldMapId,
                        principalTable: "FieldMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ContactUploadId",
                table: "Contacts",
                column: "ContactUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUploadFieldMaps_ContactUploadId",
                table: "ContactUploadFieldMaps",
                column: "ContactUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUploadFieldMaps_FieldMapId",
                table: "ContactUploadFieldMaps",
                column: "FieldMapId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUploads_GroupId",
                table: "ContactUploads",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactValueMaps_ContactId",
                table: "ContactValueMaps",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactValueMaps_FieldMapId",
                table: "ContactValueMaps",
                column: "FieldMapId");

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

            migrationBuilder.DropTable(
                name: "ContactUploadFieldMaps");

            migrationBuilder.DropTable(
                name: "ContactValueMaps");

            migrationBuilder.DropTable(
                name: "ContactUploads");

            migrationBuilder.DropTable(
                name: "FieldMaps");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_ContactUploadId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ContactUploadId",
                table: "Contacts");

            migrationBuilder.CreateTable(
                name: "SingleValueEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleValueEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleValueEntries_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleValueEntries_ContactId",
                table: "SingleValueEntries",
                column: "ContactId");
        }
    }
}
