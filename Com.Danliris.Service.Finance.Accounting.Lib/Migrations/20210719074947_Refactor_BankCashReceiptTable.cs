using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Refactor_BankCashReceiptTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankCashReceiptItemModel_BankCashReceiptModel_BankCashReceiptId",
                table: "BankCashReceiptItemModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankCashReceiptModel",
                table: "BankCashReceiptModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankCashReceiptItemModel",
                table: "BankCashReceiptItemModel");

            migrationBuilder.RenameTable(
                name: "BankCashReceiptModel",
                newName: "GarmentFinanceBankCashReceipts");

            migrationBuilder.RenameTable(
                name: "BankCashReceiptItemModel",
                newName: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.RenameIndex(
                name: "IX_BankCashReceiptItemModel_BankCashReceiptId",
                table: "GarmentFinanceBankCashReceiptItems",
                newName: "IX_GarmentFinanceBankCashReceiptItems_BankCashReceiptId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GarmentFinanceBankCashReceipts",
                table: "GarmentFinanceBankCashReceipts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GarmentFinanceBankCashReceiptItems",
                table: "GarmentFinanceBankCashReceiptItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentFinanceBankCashReceiptItems_GarmentFinanceBankCashReceipts_BankCashReceiptId",
                table: "GarmentFinanceBankCashReceiptItems",
                column: "BankCashReceiptId",
                principalTable: "GarmentFinanceBankCashReceipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentFinanceBankCashReceiptItems_GarmentFinanceBankCashReceipts_BankCashReceiptId",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GarmentFinanceBankCashReceipts",
                table: "GarmentFinanceBankCashReceipts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GarmentFinanceBankCashReceiptItems",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.RenameTable(
                name: "GarmentFinanceBankCashReceipts",
                newName: "BankCashReceiptModel");

            migrationBuilder.RenameTable(
                name: "GarmentFinanceBankCashReceiptItems",
                newName: "BankCashReceiptItemModel");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentFinanceBankCashReceiptItems_BankCashReceiptId",
                table: "BankCashReceiptItemModel",
                newName: "IX_BankCashReceiptItemModel_BankCashReceiptId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankCashReceiptModel",
                table: "BankCashReceiptModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankCashReceiptItemModel",
                table: "BankCashReceiptItemModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankCashReceiptItemModel_BankCashReceiptModel_BankCashReceiptId",
                table: "BankCashReceiptItemModel",
                column: "BankCashReceiptId",
                principalTable: "BankCashReceiptModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
