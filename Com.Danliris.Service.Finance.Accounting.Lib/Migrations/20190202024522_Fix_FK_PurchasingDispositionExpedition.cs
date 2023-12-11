using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Fix_FK_PurchasingDispositionExpedition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDispositionNoteDetails_PaymentDispositionNotes_PaymentDispositionNoteItemId",
                table: "PaymentDispositionNoteDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDispositionNoteDetails_PaymentDispositionNoteItems_PaymentDispositionNoteItemModelId",
                table: "PaymentDispositionNoteDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDispositionNoteDetails_PaymentDispositionNoteItemModelId",
                table: "PaymentDispositionNoteDetails");

            migrationBuilder.DropColumn(
                name: "PaymentDispositionNoteItemModelId",
                table: "PaymentDispositionNoteDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDispositionNoteDetails_PaymentDispositionNoteItems_PaymentDispositionNoteItemId",
                table: "PaymentDispositionNoteDetails",
                column: "PaymentDispositionNoteItemId",
                principalTable: "PaymentDispositionNoteItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDispositionNoteDetails_PaymentDispositionNoteItems_PaymentDispositionNoteItemId",
                table: "PaymentDispositionNoteDetails");

            migrationBuilder.AddColumn<int>(
                name: "PaymentDispositionNoteItemModelId",
                table: "PaymentDispositionNoteDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDispositionNoteDetails_PaymentDispositionNoteItemModelId",
                table: "PaymentDispositionNoteDetails",
                column: "PaymentDispositionNoteItemModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDispositionNoteDetails_PaymentDispositionNotes_PaymentDispositionNoteItemId",
                table: "PaymentDispositionNoteDetails",
                column: "PaymentDispositionNoteItemId",
                principalTable: "PaymentDispositionNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDispositionNoteDetails_PaymentDispositionNoteItems_PaymentDispositionNoteItemModelId",
                table: "PaymentDispositionNoteDetails",
                column: "PaymentDispositionNoteItemModelId",
                principalTable: "PaymentDispositionNoteItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
