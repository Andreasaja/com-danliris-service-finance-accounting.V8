using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPosted",
                table: "MemoDetailGarmentPurchasings",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MemoDetailGarmentPurchasingId",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasingId",
                table: "MemoDetailGarmentPurchasingDetails",
                column: "MemoDetailGarmentPurchasingId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasings_MemoDetailGarmentPurchasingId",
                table: "MemoDetailGarmentPurchasingDetails",
                column: "MemoDetailGarmentPurchasingId",
                principalTable: "MemoDetailGarmentPurchasings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasings_MemoDetailGarmentPurchasingId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropIndex(
                name: "IX_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasingId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropColumn(
                name: "MemoDetailGarmentPurchasingId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.AlterColumn<int>(
                name: "IsPosted",
                table: "MemoDetailGarmentPurchasings",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
