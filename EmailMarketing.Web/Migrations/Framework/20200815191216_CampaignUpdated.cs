using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class CampaignUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Campaigns_CampaignId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CampaignId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SeenDate",
                table: "CampaignReports");

            migrationBuilder.AddColumn<string>(
                name: "EmailBody",
                table: "Campaigns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSubject",
                table: "Campaigns",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Campaigns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessing",
                table: "Campaigns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSucceed",
                table: "Campaigns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDateTime",
                table: "Campaigns",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SeenDateTime",
                table: "CampaignReports",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDateTime",
                table: "CampaignReports",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailBody",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "EmailSubject",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "IsProcessing",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "IsSucceed",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "SendDateTime",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "SeenDateTime",
                table: "CampaignReports");

            migrationBuilder.DropColumn(
                name: "SendDateTime",
                table: "CampaignReports");

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SeenDate",
                table: "CampaignReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CampaignId",
                table: "Groups",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Campaigns_CampaignId",
                table: "Groups",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
