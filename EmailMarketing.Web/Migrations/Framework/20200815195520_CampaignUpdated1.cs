using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class CampaignUpdated1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailBody",
                table: "Campaigns");

            migrationBuilder.AddColumn<int>(
                name: "EmailTemplateId",
                table: "Campaigns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSendEmailNotify",
                table: "Campaigns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SendEmailAddress",
                table: "Campaigns",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DownloadQueues",
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
                    DownloadFor = table.Column<int>(nullable: false),
                    DownloadEntityId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    IsSendEmailNotify = table.Column<bool>(nullable: false),
                    SendEmailAddress = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    IsSucceed = table.Column<bool>(nullable: false),
                    SucceedEntryCount = table.Column<int>(nullable: false),
                    IsProcessing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadQueues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
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
                    UserId = table.Column<Guid>(nullable: false),
                    EmailBody = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_EmailTemplateId",
                table: "Campaigns",
                column: "EmailTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_EmailTemplates_EmailTemplateId",
                table: "Campaigns",
                column: "EmailTemplateId",
                principalTable: "EmailTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_EmailTemplates_EmailTemplateId",
                table: "Campaigns");

            migrationBuilder.DropTable(
                name: "DownloadQueues");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_EmailTemplateId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "EmailTemplateId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "IsSendEmailNotify",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "SendEmailAddress",
                table: "Campaigns");

            migrationBuilder.AddColumn<string>(
                name: "EmailBody",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
