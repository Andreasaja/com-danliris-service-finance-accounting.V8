using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Updating_PurchasingDispositionExpeditionModel_ProformaNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceNo",
                table: "PurchasingDispositionExpeditions",
                newName: "ProformaNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProformaNo",
                table: "PurchasingDispositionExpeditions",
                newName: "InvoiceNo");
        }
    }
}
