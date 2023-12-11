using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddField_Remarks_BankCashReceiptDetailsOtherItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "GarmentFinanceBankCashReceiptDetailItems");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "GarmentFinanceBankCashReceiptDetailOtherItems",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "GarmentFinanceBankCashReceiptDetailOtherItems");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "GarmentFinanceBankCashReceiptDetailItems",
                maxLength: 1000,
                nullable: true);
        }
    }
}
