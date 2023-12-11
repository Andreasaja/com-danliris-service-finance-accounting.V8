using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_Coloumn_VatId_VatRate_VBRequestDocumentItemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VatId",
                table: "VBRequestDocumentItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VatRate",
                table: "VBRequestDocumentItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatId",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "VBRequestDocumentItems");
        }
    }
}
