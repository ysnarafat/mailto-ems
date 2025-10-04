using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailMarketing.Web.Migrations.Framework
{
    public partial class FieldMapSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                   table: "FieldMaps",
                   columns: new[] { "Id", "IsActive", "IsDeleted", "Created", "IsStandard", "DisplayName" },
                   values: new object[,]
                   {
                    { 1, true, false, new DateTime(2020, 8, 1), true, "Email" },
                    { 2, true, false, new DateTime(2020, 8, 1), true, "Name" },
                    { 3, true, false, new DateTime(2020, 8, 1),  true, "Address" },
                    { 4, true, false, new DateTime(2020, 8, 1),  true, "Phone" },
                    { 5, true, false, new DateTime(2020, 8, 1),  true, "Gender" },
                    { 6, true, false, new DateTime(2020, 8, 1),  true, "DateOfBirth" }
                   });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FieldMaps",
                keyColumn: "Id",
                keyValue: 1);
            migrationBuilder.DeleteData(
                table: "FieldMaps",
                keyColumn: "Id",
                keyValue: 2);
            migrationBuilder.DeleteData(
                table: "FieldMaps",
                keyColumn: "Id",
                keyValue: 3);
            migrationBuilder.DeleteData(
                table: "FieldMaps",
                keyColumn: "Id",
                keyValue: 4);
            migrationBuilder.DeleteData(
                table: "FieldMaps",
                keyColumn: "Id",
                keyValue: 5);
            migrationBuilder.DeleteData(
                table: "FieldMaps",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
