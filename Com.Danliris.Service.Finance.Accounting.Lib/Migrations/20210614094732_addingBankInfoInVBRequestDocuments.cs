using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class addingBankInfoInVBRequestDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountCOA",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankBankCode",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankBankName",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankCode",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankCurrencyCode",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankCurrencyDescription",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankCurrencyId",
                table: "VBRequestDocuments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "BankCurrencyRate",
                table: "VBRequestDocuments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "BankCurrencySymbol",
                table: "VBRequestDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "VBRequestDocuments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountCOA",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankAccountName",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankBankCode",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankBankName",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankCode",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankCurrencyCode",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankCurrencyDescription",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankCurrencyId",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankCurrencyRate",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankCurrencySymbol",
                table: "VBRequestDocuments");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "VBRequestDocuments");
        }
    }
}
