using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class MissingField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillsNo",
                table: "DPPVATBankExpenditureNoteDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryOrdersNo",
                table: "DPPVATBankExpenditureNoteDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentBills",
                table: "DPPVATBankExpenditureNoteDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillsNo",
                table: "DPPVATBankExpenditureNoteDetails");

            migrationBuilder.DropColumn(
                name: "DeliveryOrdersNo",
                table: "DPPVATBankExpenditureNoteDetails");

            migrationBuilder.DropColumn(
                name: "PaymentBills",
                table: "DPPVATBankExpenditureNoteDetails");
        }
    }
}
