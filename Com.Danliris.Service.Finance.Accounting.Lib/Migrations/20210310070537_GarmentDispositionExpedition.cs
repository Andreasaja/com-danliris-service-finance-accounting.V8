using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class GarmentDispositionExpedition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GarmentDispositionExpeditionModel",
                table: "GarmentDispositionExpeditionModel");

            migrationBuilder.DropColumn(
                name: "DispositionId",
                table: "GarmentDispositionExpeditionModel");

            migrationBuilder.DropColumn(
                name: "DispositionNo",
                table: "GarmentDispositionExpeditionModel");

            migrationBuilder.DropColumn(
                name: "SupplierIsImport",
                table: "GarmentDispositionExpeditionModel");

            migrationBuilder.RenameTable(
                name: "GarmentDispositionExpeditionModel",
                newName: "GarmentDispositionExpeditions");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "GarmentDispositionExpeditions",
                newName: "VATAmount");

            migrationBuilder.RenameColumn(
                name: "PurchasedBy",
                table: "GarmentDispositionExpeditions",
                newName: "SendToPurchasingRemark");

            migrationBuilder.RenameColumn(
                name: "InvoiceNo",
                table: "GarmentDispositionExpeditions",
                newName: "Remark");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "GarmentDispositionExpeditions",
                newName: "DispositionNoteId");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "GarmentDispositionExpeditions",
                newName: "DispositionNoteDueDate");

            migrationBuilder.RenameColumn(
                name: "DispositionDate",
                table: "GarmentDispositionExpeditions",
                newName: "DispositionNoteDate");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "GarmentDispositionExpeditions",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SupplierCode",
                table: "GarmentDispositionExpeditions",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "GarmentDispositionExpeditions",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountingAcceptedBy",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AccountingAcceptedDate",
                table: "GarmentDispositionExpeditions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CashierAcceptedBy",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CashierAcceptedDate",
                table: "GarmentDispositionExpeditions",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyDPPAmount",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyIncomeTaxAmount",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyRate",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyTotalPaid",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyVATAmount",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DPPAmount",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DispositionNoteNo",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncomeTaxAmount",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SendToAccountingBy",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SendToAccountingDate",
                table: "GarmentDispositionExpeditions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendToCashierBy",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SendToCashierDate",
                table: "GarmentDispositionExpeditions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendToPurchasingBy",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SendToPurchasingDate",
                table: "GarmentDispositionExpeditions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendToVerificationBy",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SendToVerificationDate",
                table: "GarmentDispositionExpeditions",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalPaid",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "VerificationAcceptedBy",
                table: "GarmentDispositionExpeditions",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "VerificationAcceptedDate",
                table: "GarmentDispositionExpeditions",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GarmentDispositionExpeditions",
                table: "GarmentDispositionExpeditions",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GarmentDispositionExpeditions",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "AccountingAcceptedBy",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "AccountingAcceptedDate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "CashierAcceptedBy",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "CashierAcceptedDate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "CurrencyDPPAmount",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "CurrencyIncomeTaxAmount",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "CurrencyTotalPaid",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "CurrencyVATAmount",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "DPPAmount",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "DispositionNoteNo",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "IncomeTaxAmount",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SendToAccountingBy",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SendToAccountingDate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SendToCashierBy",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SendToCashierDate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SendToPurchasingBy",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SendToPurchasingDate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SendToVerificationBy",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SendToVerificationDate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "TotalPaid",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "VerificationAcceptedBy",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "VerificationAcceptedDate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.RenameTable(
                name: "GarmentDispositionExpeditions",
                newName: "GarmentDispositionExpeditionModel");

            migrationBuilder.RenameColumn(
                name: "VATAmount",
                table: "GarmentDispositionExpeditionModel",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "SendToPurchasingRemark",
                table: "GarmentDispositionExpeditionModel",
                newName: "PurchasedBy");

            migrationBuilder.RenameColumn(
                name: "Remark",
                table: "GarmentDispositionExpeditionModel",
                newName: "InvoiceNo");

            migrationBuilder.RenameColumn(
                name: "DispositionNoteId",
                table: "GarmentDispositionExpeditionModel",
                newName: "InvoiceId");

            migrationBuilder.RenameColumn(
                name: "DispositionNoteDueDate",
                table: "GarmentDispositionExpeditionModel",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "DispositionNoteDate",
                table: "GarmentDispositionExpeditionModel",
                newName: "DispositionDate");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "GarmentDispositionExpeditionModel",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SupplierCode",
                table: "GarmentDispositionExpeditionModel",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "GarmentDispositionExpeditionModel",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DispositionId",
                table: "GarmentDispositionExpeditionModel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DispositionNo",
                table: "GarmentDispositionExpeditionModel",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupplierIsImport",
                table: "GarmentDispositionExpeditionModel",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GarmentDispositionExpeditionModel",
                table: "GarmentDispositionExpeditionModel",
                column: "Id");
        }
    }
}
