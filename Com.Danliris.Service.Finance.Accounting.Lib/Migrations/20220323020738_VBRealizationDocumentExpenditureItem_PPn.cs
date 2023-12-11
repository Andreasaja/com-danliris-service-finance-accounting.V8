using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class VBRealizationDocumentExpenditureItem_PPn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VatId",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VatRate",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatId",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "VBRealizationDocumentExpenditureItems");
        }
    }
}
