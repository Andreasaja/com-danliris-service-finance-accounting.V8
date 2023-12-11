using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddingUnitInInvoiceNI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitId",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices");
        }
    }
}
