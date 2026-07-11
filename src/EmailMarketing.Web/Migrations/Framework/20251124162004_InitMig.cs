using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmailMarketing.Web.Migrations.Framework
{
    /// <inheritdoc />
    public partial class InitMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    IsSucceed = table.Column<bool>(type: "boolean", nullable: false),
                    IsUpdateExisting = table.Column<bool>(type: "boolean", nullable: false),
                    HasColumnHeader = table.Column<bool>(type: "boolean", nullable: false),
                    IsSendEmailNotify = table.Column<bool>(type: "boolean", nullable: false),
                    SendEmailAddress = table.Column<string>(type: "text", nullable: true),
                    SucceedEntryCount = table.Column<int>(type: "integer", nullable: false),
                    IsProcessing = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DownloadQueues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DownloadQueueFor = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSendEmailNotify = table.Column<bool>(type: "boolean", nullable: false),
                    SendEmailAddress = table.Column<string>(type: "text", nullable: true),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    IsSucceed = table.Column<bool>(type: "boolean", nullable: false),
                    SucceedEntryCount = table.Column<int>(type: "integer", nullable: false),
                    IsProcessing = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadQueues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailTemplateName = table.Column<string>(type: "text", nullable: true),
                    EmailTemplateBody = table.Column<string>(type: "text", nullable: true),
                    IsPersonalized = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    IsStandard = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SMTPConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Server = table.Column<string>(type: "text", nullable: true),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    SenderName = table.Column<string>(type: "text", nullable: true),
                    SenderEmail = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    EnableSSL = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMTPConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactUploadId = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_ContactUploads_ContactUploadId",
                        column: x => x.ContactUploadId,
                        principalTable: "ContactUploads",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DownloadQueueSubEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DownloadQueueId = table.Column<int>(type: "integer", nullable: false),
                    DownloadQueueSubEntityId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadQueueSubEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownloadQueueSubEntities_DownloadQueues_DownloadQueueId",
                        column: x => x.DownloadQueueId,
                        principalTable: "DownloadQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactUploadFieldMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    FieldMapId = table.Column<int>(type: "integer", nullable: false),
                    ContactUploadId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "ContactUploadGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactUploadId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EmailSubject = table.Column<string>(type: "text", nullable: true),
                    EmailTemplateId = table.Column<int>(type: "integer", nullable: false),
                    SendDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSendEmailNotify = table.Column<bool>(type: "boolean", nullable: false),
                    SendEmailAddress = table.Column<string>(type: "text", nullable: true),
                    IsDraft = table.Column<bool>(type: "boolean", nullable: false),
                    IsProcessing = table.Column<bool>(type: "boolean", nullable: false),
                    IsSucceed = table.Column<bool>(type: "boolean", nullable: false),
                    SMTPConfigId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPersonalized = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_EmailTemplates_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "EmailTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Campaigns_SMTPConfigs_SMTPConfigId",
                        column: x => x.SMTPConfigId,
                        principalTable: "SMTPConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "ContactValueMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: true),
                    ContactId = table.Column<int>(type: "integer", nullable: false),
                    FieldMapId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "CampaignGroups",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false)
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
                name: "CampaignReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CampaignId = table.Column<int>(type: "integer", nullable: false),
                    ContactId = table.Column<int>(type: "integer", nullable: false),
                    SMTPConfigId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDelivered = table.Column<bool>(type: "boolean", nullable: false),
                    IsSeen = table.Column<bool>(type: "boolean", nullable: false),
                    IsPersonalized = table.Column<bool>(type: "boolean", nullable: false),
                    SendDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SeenDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "IX_Campaigns_EmailTemplateId",
                table: "Campaigns",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_SMTPConfigId",
                table: "Campaigns",
                column: "SMTPConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactGroups_ContactId",
                table: "ContactGroups",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactGroups_GroupId",
                table: "ContactGroups",
                column: "GroupId");

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
                name: "IX_ContactUploadGroups_ContactUploadId",
                table: "ContactUploadGroups",
                column: "ContactUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUploadGroups_GroupId",
                table: "ContactUploadGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactValueMaps_ContactId",
                table: "ContactValueMaps",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactValueMaps_FieldMapId",
                table: "ContactValueMaps",
                column: "FieldMapId");

            migrationBuilder.CreateIndex(
                name: "IX_DownloadQueueSubEntities_DownloadQueueId",
                table: "DownloadQueueSubEntities",
                column: "DownloadQueueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignGroups");

            migrationBuilder.DropTable(
                name: "CampaignReports");

            migrationBuilder.DropTable(
                name: "ContactGroups");

            migrationBuilder.DropTable(
                name: "ContactUploadFieldMaps");

            migrationBuilder.DropTable(
                name: "ContactUploadGroups");

            migrationBuilder.DropTable(
                name: "ContactValueMaps");

            migrationBuilder.DropTable(
                name: "DownloadQueueSubEntities");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "FieldMaps");

            migrationBuilder.DropTable(
                name: "DownloadQueues");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "SMTPConfigs");

            migrationBuilder.DropTable(
                name: "ContactUploads");
        }
    }
}
