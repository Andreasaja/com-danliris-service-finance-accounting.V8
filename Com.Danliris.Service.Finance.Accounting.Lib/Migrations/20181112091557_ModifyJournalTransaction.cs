using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class ModifyJournalTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalTransactionItems_JournalTransactions_JournalTransactionModelId",
                table: "JournalTransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_JournalTransactionItems_JournalTransactionModelId",
                table: "JournalTransactionItems");

            migrationBuilder.DropColumn(
                name: "JournalTransactionModelId",
                table: "JournalTransactionItems");

            migrationBuilder.AddColumn<int>(
                name: "JournalTransactionId",
                table: "JournalTransactionItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JournalTransactionItems_JournalTransactionId",
                table: "JournalTransactionItems",
                column: "JournalTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalTransactionItems_JournalTransactions_JournalTransactionId",
                table: "JournalTransactionItems",
                column: "JournalTransactionId",
                principalTable: "JournalTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalTransactionItems_JournalTransactions_JournalTransactionId",
                table: "JournalTransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_JournalTransactionItems_JournalTransactionId",
                table: "JournalTransactionItems");

            migrationBuilder.DropColumn(
                name: "JournalTransactionId",
                table: "JournalTransactionItems");

            migrationBuilder.AddColumn<int>(
                name: "JournalTransactionModelId",
                table: "JournalTransactionItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalTransactionItems_JournalTransactionModelId",
                table: "JournalTransactionItems",
                column: "JournalTransactionModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalTransactionItems_JournalTransactions_JournalTransactionModelId",
                table: "JournalTransactionItems",
                column: "JournalTransactionModelId",
                principalTable: "JournalTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
