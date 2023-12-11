using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_Migrations_MemoDetailGarmentPurchasingModel_MemoDetailGarmentPurchasingDetailModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_MemoDetailGarmentPurchasingDetails_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails",
                column: "MemoDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasings_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails",
                column: "MemoDetailId",
                principalTable: "MemoDetailGarmentPurchasings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasings_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropIndex(
                name: "IX_MemoDetailGarmentPurchasingDetails_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails");

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
    }
}
