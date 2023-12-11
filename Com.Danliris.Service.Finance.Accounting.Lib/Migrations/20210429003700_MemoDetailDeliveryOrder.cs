using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class MemoDetailDeliveryOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PurchasingRate",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "PaymentRate",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "BillsNo",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalNoteNo",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentBills",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PurchaseAmount",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillsNo",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropColumn(
                name: "InternalNoteNo",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropColumn(
                name: "PaymentBills",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseAmount",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PurchasingRate",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "PaymentRate",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
