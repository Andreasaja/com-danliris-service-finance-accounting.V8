using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Update_MemoGarmentPurchasingDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoGarmentDetailPurchasings_MemoGarmentPurchasings_MemoId",
                table: "MemoGarmentDetailPurchasings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemoGarmentDetailPurchasings",
                table: "MemoGarmentDetailPurchasings");

            migrationBuilder.RenameTable(
                name: "MemoGarmentDetailPurchasings",
                newName: "MemoGarmentPurchasingDetails");

            migrationBuilder.RenameIndex(
                name: "IX_MemoGarmentDetailPurchasings_MemoId",
                table: "MemoGarmentPurchasingDetails",
                newName: "IX_MemoGarmentPurchasingDetails_MemoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemoGarmentPurchasingDetails",
                table: "MemoGarmentPurchasingDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoGarmentPurchasingDetails_MemoGarmentPurchasings_MemoId",
                table: "MemoGarmentPurchasingDetails",
                column: "MemoId",
                principalTable: "MemoGarmentPurchasings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoGarmentPurchasingDetails_MemoGarmentPurchasings_MemoId",
                table: "MemoGarmentPurchasingDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemoGarmentPurchasingDetails",
                table: "MemoGarmentPurchasingDetails");

            migrationBuilder.RenameTable(
                name: "MemoGarmentPurchasingDetails",
                newName: "MemoGarmentDetailPurchasings");

            migrationBuilder.RenameIndex(
                name: "IX_MemoGarmentPurchasingDetails_MemoId",
                table: "MemoGarmentDetailPurchasings",
                newName: "IX_MemoGarmentDetailPurchasings_MemoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemoGarmentDetailPurchasings",
                table: "MemoGarmentDetailPurchasings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoGarmentDetailPurchasings_MemoGarmentPurchasings_MemoId",
                table: "MemoGarmentDetailPurchasings",
                column: "MemoId",
                principalTable: "MemoGarmentPurchasings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
