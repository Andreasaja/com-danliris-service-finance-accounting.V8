using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerifiedBy",
                table: "VBRealizationDocumentExpeditions",
                newName: "VerifiedToCashierBy");

            migrationBuilder.RenameColumn(
                name: "VerifedDate",
                table: "VBRealizationDocumentExpeditions",
                newName: "VerifiedToCashierDate");

            migrationBuilder.RenameColumn(
                name: "SendToCashierDate",
                table: "VBRealizationDocumentExpeditions",
                newName: "VerificationReceiptDate");

            migrationBuilder.RenameColumn(
                name: "SendToCashierBy",
                table: "VBRealizationDocumentExpeditions",
                newName: "VerificationReceiptBy");

            migrationBuilder.RenameColumn(
                name: "CashierDate",
                table: "VBRealizationDocumentExpeditions",
                newName: "CashierReceiptDate");

            migrationBuilder.RenameColumn(
                name: "CashierBy",
                table: "VBRealizationDocumentExpeditions",
                newName: "CashierReceiptBy");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "VBRealizationDocumentExpeditions",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyRate",
                table: "VBRealizationDocumentExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "RealizationVbs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VBId",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "VBRealizationDocumentExpeditions");

            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                table: "VBRealizationDocumentExpeditions");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "VBId",
                table: "RealizationVbs");

            migrationBuilder.RenameColumn(
                name: "VerifiedToCashierDate",
                table: "VBRealizationDocumentExpeditions",
                newName: "VerifedDate");

            migrationBuilder.RenameColumn(
                name: "VerifiedToCashierBy",
                table: "VBRealizationDocumentExpeditions",
                newName: "VerifiedBy");

            migrationBuilder.RenameColumn(
                name: "VerificationReceiptDate",
                table: "VBRealizationDocumentExpeditions",
                newName: "SendToCashierDate");

            migrationBuilder.RenameColumn(
                name: "VerificationReceiptBy",
                table: "VBRealizationDocumentExpeditions",
                newName: "SendToCashierBy");

            migrationBuilder.RenameColumn(
                name: "CashierReceiptDate",
                table: "VBRealizationDocumentExpeditions",
                newName: "CashierDate");

            migrationBuilder.RenameColumn(
                name: "CashierReceiptBy",
                table: "VBRealizationDocumentExpeditions",
                newName: "CashierBy");
        }
    }
}
