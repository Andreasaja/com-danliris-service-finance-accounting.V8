using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddVBRealizationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VBDocumentLayoutOrder",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "VBRealizationDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "VBRealizationDocuments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VBDocumentLayoutOrder",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "VBRealizationDocuments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "VBRealizationDocuments");
        }
    }
}
