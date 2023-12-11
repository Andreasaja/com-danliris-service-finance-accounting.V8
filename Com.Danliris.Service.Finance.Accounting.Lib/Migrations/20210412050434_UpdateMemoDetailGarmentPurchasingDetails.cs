using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateMemoDetailGarmentPurchasingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupplierCode",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierCode",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "MemoDetailGarmentPurchasingDetails");
        }
    }
}
