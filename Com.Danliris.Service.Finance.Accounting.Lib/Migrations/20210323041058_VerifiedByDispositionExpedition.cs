using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class VerifiedByDispositionExpedition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerifiedBy",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "VerifiedDate",
                table: "GarmentDispositionExpeditions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "VerifiedDate",
                table: "GarmentDispositionExpeditions");
        }
    }
}
