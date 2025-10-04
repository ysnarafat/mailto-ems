using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class CampaignUpdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPersonalized",
                table: "Campaigns",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPersonalized",
                table: "Campaigns");
        }
    }
}
