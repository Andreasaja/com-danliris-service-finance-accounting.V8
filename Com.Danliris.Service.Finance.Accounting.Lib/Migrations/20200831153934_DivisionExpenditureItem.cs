using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class DivisionExpenditureItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                table: "VBRealizationDocumentExpenditureItems",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "VBRealizationDocumentExpenditureItems",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisionCode",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "VBRealizationDocumentExpenditureItems");
        }
    }
}
