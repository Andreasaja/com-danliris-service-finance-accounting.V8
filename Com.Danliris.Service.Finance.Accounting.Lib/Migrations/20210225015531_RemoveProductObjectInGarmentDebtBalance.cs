using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class RemoveProductObjectInGarmentDebtBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GarmentDebtBalances");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "GarmentDebtBalances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GarmentDebtBalances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "GarmentDebtBalances",
                maxLength: 128,
                nullable: true);
        }
    }
}
