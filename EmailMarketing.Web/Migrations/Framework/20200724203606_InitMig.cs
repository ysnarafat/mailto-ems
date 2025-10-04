using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class InitMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaigns",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SMTPConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<Guid>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Server = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    SenderName = table.Column<string>(nullable: true),
                    SenderEmail = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    EnableSSL = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMTPConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
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
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    CampaignId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CampaignGroups",
                columns: table => new
                {
                    CampaignId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignGroups", x => new { x.CampaignId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_CampaignGroups_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
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
                    Email = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignReports",
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
                    CampaignId = table.Column<int>(nullable: false),
                    ContactId = table.Column<int>(nullable: false),
                    SMTPConfigId = table.Column<Guid>(nullable: false),
                    IsDelivered = table.Column<bool>(nullable: false),
                    IsSeen = table.Column<bool>(nullable: false),
                    IsPersonalized = table.Column<bool>(nullable: false),
                    SeenDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignReports_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignReports_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignReports_SMTPConfigs_SMTPConfigId",
                        column: x => x.SMTPConfigId,
                        principalTable: "SMTPConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SingleValueEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: false)
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
                name: "IX_CampaignGroups_GroupId",
                table: "CampaignGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignReports_CampaignId",
                table: "CampaignReports",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignReports_ContactId",
                table: "CampaignReports",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignReports_SMTPConfigId",
                table: "CampaignReports",
                column: "SMTPConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_GroupId",
                table: "Contacts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CampaignId",
                table: "Groups",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleValueEntries_ContactId",
                table: "SingleValueEntries",
                column: "ContactId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignGroups");

            migrationBuilder.DropTable(
                name: "CampaignReports");

            migrationBuilder.DropTable(
                name: "SingleValueEntries");

            migrationBuilder.DropTable(
                name: "SMTPConfigs");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Campaigns");
        }
    }
}
