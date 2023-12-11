using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddField_TypeAmount_BankCashReceiptDetailItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeAmount",
                table: "GarmentFinanceBankCashReceiptDetailOtherItems",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeAmount",
                table: "GarmentFinanceBankCashReceiptDetailOtherItems");
        }
    }
}
