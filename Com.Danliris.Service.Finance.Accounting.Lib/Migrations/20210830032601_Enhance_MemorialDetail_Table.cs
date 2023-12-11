using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Enhance_MemorialDetail_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "GarmentFinanceMemorialDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DebitCoaCode",
                table: "GarmentFinanceMemorialDetails",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DebitCoaId",
                table: "GarmentFinanceMemorialDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DebitCoaName",
                table: "GarmentFinanceMemorialDetails",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceCoaCode",
                table: "GarmentFinanceMemorialDetails",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceCoaId",
                table: "GarmentFinanceMemorialDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceCoaName",
                table: "GarmentFinanceMemorialDetails",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "GarmentFinanceMemorialDetailOtherItems",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeAmount",
                table: "GarmentFinanceMemorialDetailOtherItems",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "GarmentFinanceMemorialDetails");

            migrationBuilder.DropColumn(
                name: "DebitCoaCode",
                table: "GarmentFinanceMemorialDetails");

            migrationBuilder.DropColumn(
                name: "DebitCoaId",
                table: "GarmentFinanceMemorialDetails");

            migrationBuilder.DropColumn(
                name: "DebitCoaName",
                table: "GarmentFinanceMemorialDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceCoaCode",
                table: "GarmentFinanceMemorialDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceCoaId",
                table: "GarmentFinanceMemorialDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceCoaName",
                table: "GarmentFinanceMemorialDetails");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "GarmentFinanceMemorialDetailOtherItems");

            migrationBuilder.DropColumn(
                name: "TypeAmount",
                table: "GarmentFinanceMemorialDetailOtherItems");
        }
    }
}
