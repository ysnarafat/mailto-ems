using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class DownloadQueueEntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadEntityId",
                table: "DownloadQueues");

            migrationBuilder.DropColumn(
                name: "DownloadFor",
                table: "DownloadQueues");

            migrationBuilder.AddColumn<string>(
                name: "EmailTemplateName",
                table: "EmailTemplates",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DownloadQueueFor",
                table: "DownloadQueues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DownloadQueueSubEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DownloadQueueId = table.Column<int>(nullable: false),
                    DownloadQueueSubEntityId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_DownloadQueueSubEntities_DownloadQueueId",
                table: "DownloadQueueSubEntities",
                column: "DownloadQueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadQueueSubEntities");

            migrationBuilder.DropColumn(
                name: "EmailTemplateName",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DownloadQueueFor",
                table: "DownloadQueues");

            migrationBuilder.AddColumn<int>(
                name: "DownloadEntityId",
                table: "DownloadQueues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DownloadFor",
                table: "DownloadQueues",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
