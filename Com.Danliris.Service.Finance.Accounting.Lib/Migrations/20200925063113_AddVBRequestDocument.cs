using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddVBRequestDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoBL",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoPO",
                table: "VBRequestDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoBL",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "NoPO",
                table: "VBRequestDocuments");
        }
    }
}
