using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class VBRealizationIsCompleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletedBy",
                table: "VBRealizationDocuments",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CompletedDate",
                table: "VBRealizationDocuments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "VBRealizationDocuments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedBy",
                table: "VBRealizationDocuments");

            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "VBRealizationDocuments");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "VBRealizationDocuments");
        }
    }
}
