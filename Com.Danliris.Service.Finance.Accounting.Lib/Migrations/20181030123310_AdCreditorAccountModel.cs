using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AdCreditorAccountModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CashAccount",
                table: "ChartsOfAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nature",
                table: "ChartsOfAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportType",
                table: "ChartsOfAccounts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreditorAccounts",
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
                    SupplierName = table.Column<string>(nullable: true),
                    SupplierCode = table.Column<string>(nullable: true),
                    UnitReceiptNoteNo = table.Column<string>(nullable: true),
                    UnitReceiptNoteDate = table.Column<DateTimeOffset>(nullable: true),
                    UnitReceiptNoteDPP = table.Column<long>(nullable: false),
                    UnitReceiptNotePPN = table.Column<long>(nullable: false),
                    UnitReceiptMutation = table.Column<long>(nullable: false),
                    BankExpenditureNoteId = table.Column<int>(nullable: false),
                    BankExpenditureNoteNo = table.Column<string>(nullable: true),
                    BankExpenditureNoteDate = table.Column<DateTimeOffset>(nullable: true),
                    BankExpenditureNoteDPP = table.Column<long>(nullable: false),
                    BankExpenditureNotePPN = table.Column<long>(nullable: false),
                    BankExpenditureNoteMutation = table.Column<long>(nullable: false),
                    MemoNo = table.Column<string>(nullable: true),
                    MemoDate = table.Column<DateTimeOffset>(nullable: true),
                    MemoDPP = table.Column<long>(nullable: false),
                    MemoPPN = table.Column<long>(nullable: false),
                    MemoMutation = table.Column<long>(nullable: false),
                    InvoiceNo = table.Column<string>(nullable: true),
                    FinalBalance = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditorAccounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "CashAccount",
                table: "ChartsOfAccounts");

            migrationBuilder.DropColumn(
                name: "Nature",
                table: "ChartsOfAccounts");

            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "ChartsOfAccounts");
        }
    }
}
