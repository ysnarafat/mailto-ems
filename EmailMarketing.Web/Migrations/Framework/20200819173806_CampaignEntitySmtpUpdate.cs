using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class CampaignEntitySmtpUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SMTPConfigId",
                table: "Campaigns",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_SMTPConfigId",
                table: "Campaigns",
                column: "SMTPConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_SMTPConfigs_SMTPConfigId",
                table: "Campaigns",
                column: "SMTPConfigId",
                principalTable: "SMTPConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_SMTPConfigs_SMTPConfigId",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_SMTPConfigId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "SMTPConfigId",
                table: "Campaigns");
        }
    }
}
