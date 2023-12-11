using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class addingDOInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillNo",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoNo",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentBill",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNo",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices");

            migrationBuilder.DropColumn(
                name: "DoNo",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices");

            migrationBuilder.DropColumn(
                name: "PaymentBill",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices");
        }
    }
}
