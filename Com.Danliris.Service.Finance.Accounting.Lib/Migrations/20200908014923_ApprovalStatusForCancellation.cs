using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class ApprovalStatusForCancellation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "VBRequestDocuments");

            migrationBuilder.RenameColumn(
                name: "ApprovedDate",
                table: "VBRequestDocuments",
                newName: "CancellationDate");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ApprovalDate",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "VBRequestDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CanceledBy",
                table: "VBRequestDocuments",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "CanceledBy",
                table: "VBRequestDocuments");

            migrationBuilder.RenameColumn(
                name: "CancellationDate",
                table: "VBRequestDocuments",
                newName: "ApprovedDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "VBRequestDocuments",
                nullable: false,
                defaultValue: false);
        }
    }
}
