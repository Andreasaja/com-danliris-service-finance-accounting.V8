using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class DailyBankTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankTransactionMonthlyBalances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    InitialBalance = table.Column<double>(nullable: false),
                    RemainingBalance = table.Column<double>(nullable: false),
                    AccountBankId = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactionMonthlyBalances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyBankTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    AccountBankId = table.Column<string>(maxLength: 50, nullable: true),
                    AccountBankCode = table.Column<string>(maxLength: 25, nullable: true),
                    AccountBankName = table.Column<string>(maxLength: 100, nullable: true),
                    AccountBankAccountName = table.Column<string>(maxLength: 100, nullable: true),
                    AccountBankAccountNumber = table.Column<string>(maxLength: 100, nullable: true),
                    AccountBankCurrencyCode = table.Column<string>(maxLength: 100, nullable: true),
                    AccountBankCurrencyId = table.Column<string>(maxLength: 50, nullable: true),
                    AccountBankCurrencySymbol = table.Column<string>(maxLength: 100, nullable: true),
                    Code = table.Column<string>(maxLength: 25, nullable: true),
                    BuyerCode = table.Column<string>(maxLength: 25, nullable: true),
                    BuyerId = table.Column<string>(maxLength: 50, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 150, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Nominal = table.Column<double>(nullable: false),
                    ReferenceNo = table.Column<string>(maxLength: 50, nullable: true),
                    ReferenceType = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    SourceType = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    SupplierId = table.Column<string>(maxLength: 50, nullable: true),
                    SupplierCode = table.Column<string>(maxLength: 100, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 250, nullable: true),
                    AfterNominal = table.Column<double>(nullable: false),
                    BeforeNominal = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyBankTransactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransactionMonthlyBalances");

            migrationBuilder.DropTable(
                name: "DailyBankTransactions");
        }
    }
}
