using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddingMemoInfoInGarmentDebtBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MemoAmount",
                table: "GarmentDebtBalances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MemoDetailId",
                table: "GarmentDebtBalances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MemoNo",
                table: "GarmentDebtBalances",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PaymentRate",
                table: "GarmentDebtBalances",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemoAmount",
                table: "GarmentDebtBalances");

            migrationBuilder.DropColumn(
                name: "MemoDetailId",
                table: "GarmentDebtBalances");

            migrationBuilder.DropColumn(
                name: "MemoNo",
                table: "GarmentDebtBalances");

            migrationBuilder.DropColumn(
                name: "PaymentRate",
                table: "GarmentDebtBalances");
        }
    }
}
