using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class EnhanceVBRequestAndVBRealization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ApprovedDate",
                table: "VBRequestDocuments",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddColumn<string>(
                name: "CompletedBy",
                table: "VBRequestDocuments",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CompletedDate",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "VBRealizationDocuments",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedBy",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "VBRealizationDocuments");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ApprovedDate",
                table: "VBRequestDocuments",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }
    }
}
