using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class smtpConfigextend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "SMTPConfigs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SMTPConfigs");
        }
    }
}
