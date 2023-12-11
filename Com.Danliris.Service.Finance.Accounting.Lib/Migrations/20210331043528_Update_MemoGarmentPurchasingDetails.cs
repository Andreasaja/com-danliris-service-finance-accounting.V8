using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Update_MemoGarmentPurchasingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasings_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemoDetailGarmentPurchasings",
                table: "MemoDetailGarmentPurchasings");

            migrationBuilder.RenameTable(
                name: "MemoDetailGarmentPurchasings",
                newName: "MemoGarmentPurchasingDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemoGarmentPurchasingDetails",
                table: "MemoGarmentPurchasingDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoGarmentPurchasingDetails_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails",
                column: "MemoDetailId",
                principalTable: "MemoGarmentPurchasingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoGarmentPurchasingDetails_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemoGarmentPurchasingDetails",
                table: "MemoGarmentPurchasingDetails");

            migrationBuilder.RenameTable(
                name: "MemoGarmentPurchasingDetails",
                newName: "MemoDetailGarmentPurchasings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemoDetailGarmentPurchasings",
                table: "MemoDetailGarmentPurchasings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasings_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails",
                column: "MemoDetailId",
                principalTable: "MemoDetailGarmentPurchasings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
