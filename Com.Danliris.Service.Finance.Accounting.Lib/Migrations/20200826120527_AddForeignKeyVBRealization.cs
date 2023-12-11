using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddForeignKeyVBRealization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VBRealizationDocumentId",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VBRealizationDocumentId",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VBRealizationDocumentId",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "VBRealizationDocumentId",
                table: "VBRealizationDocumentExpenditureItems");
        }
    }
}
