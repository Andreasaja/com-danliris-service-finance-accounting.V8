using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UnitPaymentOrderNo",
                table: "VBRealizationDocumentExpenditureItems",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierCode",
                table: "VBRealizationDocumentExpenditureItems",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "VBRealizationDocumentExpenditureItems",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierCode",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.AlterColumn<string>(
                name: "UnitPaymentOrderNo",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
