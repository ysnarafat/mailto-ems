using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class FieldMapUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "FieldMaps",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "FieldMaps",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "FieldMaps",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "FieldMaps",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "FieldMaps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "FieldMaps");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FieldMaps");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "FieldMaps");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "FieldMaps");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "FieldMaps",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
