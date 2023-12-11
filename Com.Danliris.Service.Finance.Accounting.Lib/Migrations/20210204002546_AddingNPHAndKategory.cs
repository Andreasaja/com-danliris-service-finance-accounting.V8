using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddingNPHAndKategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupplierCode",
                table: "GarmentPurchasingPphBankExpenditureNoteItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NPH",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierCode",
                table: "GarmentPurchasingPphBankExpenditureNoteItems");

            migrationBuilder.DropColumn(
                name: "NPH",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices");
        }
    }
}
