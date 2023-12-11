using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddingFieldInGarmentInvoicePurchaseDispositionRequireForDailyBankTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BankAccount",
                table: "GarmentInvoicePurchasingDispositions",
                newName: "BankCurrencyId");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                table: "GarmentInvoicePurchasingDispositions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNo",
                table: "GarmentInvoicePurchasingDispositions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankCurrencyCode",
                table: "GarmentInvoicePurchasingDispositions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankSwiftCode",
                table: "GarmentInvoicePurchasingDispositions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbol",
                table: "GarmentInvoicePurchasingDispositions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "GarmentInvoicePurchasingDispositions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PurchasingDispositionExpeditionId",
                table: "GarmentInvoicePurchasingDispositionItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BankExpenditureNoteDate",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "BankExpenditureNoteNo",
                table: "GarmentDispositionExpeditions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "GarmentDispositionExpeditions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountName",
                table: "GarmentInvoicePurchasingDispositions");

            migrationBuilder.DropColumn(
                name: "BankAccountNo",
                table: "GarmentInvoicePurchasingDispositions");

            migrationBuilder.DropColumn(
                name: "BankCurrencyCode",
                table: "GarmentInvoicePurchasingDispositions");

            migrationBuilder.DropColumn(
                name: "BankSwiftCode",
                table: "GarmentInvoicePurchasingDispositions");

            migrationBuilder.DropColumn(
                name: "CurrencySymbol",
                table: "GarmentInvoicePurchasingDispositions");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "GarmentInvoicePurchasingDispositions");

            migrationBuilder.DropColumn(
                name: "PurchasingDispositionExpeditionId",
                table: "GarmentInvoicePurchasingDispositionItems");

            migrationBuilder.DropColumn(
                name: "BankExpenditureNoteDate",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "BankExpenditureNoteNo",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "GarmentDispositionExpeditions");

            migrationBuilder.RenameColumn(
                name: "BankCurrencyId",
                table: "GarmentInvoicePurchasingDispositions",
                newName: "BankAccount");
        }
    }
}
