using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class addingSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupplierCode",
                table: "GarmentDebtBalances",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupplierIsImport",
                table: "GarmentDebtBalances",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierCode",
                table: "GarmentDebtBalances");

            migrationBuilder.DropColumn(
                name: "SupplierIsImport",
                table: "GarmentDebtBalances");
        }
    }
}
