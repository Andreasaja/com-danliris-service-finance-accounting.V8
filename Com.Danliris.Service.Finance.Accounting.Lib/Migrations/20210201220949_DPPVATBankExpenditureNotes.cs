using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class DPPVATBankExpenditureNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DPPVATBankExpenditureNoteDetails",
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
                    DPPVATBankExpenditureNoteId = table.Column<int>(nullable: false),
                    DPPVATBankExpenditureNoteItemId = table.Column<int>(nullable: false),
                    InvoiceId = table.Column<int>(nullable: false),
                    InvoiceNo = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTimeOffset>(nullable: false),
                    ProductNames = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DPPVATBankExpenditureNoteDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DPPVATBankExpenditureNoteItems",
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
                    DPPVATBankExpenditureNoteId = table.Column<int>(nullable: false),
                    InternalNoteId = table.Column<int>(nullable: false),
                    InternalNoteNo = table.Column<string>(nullable: true),
                    InternalNoteDate = table.Column<DateTimeOffset>(nullable: false),
                    DueDate = table.Column<DateTimeOffset>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierName = table.Column<string>(nullable: true),
                    IsImportSupplier = table.Column<bool>(nullable: false),
                    VATAmount = table.Column<double>(nullable: false),
                    IncomeTaxAmount = table.Column<double>(nullable: false),
                    DPP = table.Column<double>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true),
                    OutstandingAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DPPVATBankExpenditureNoteItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DPPVATBankExpenditureNotes",
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
                    DocumentNo = table.Column<string>(nullable: true),
                    BankAccountId = table.Column<int>(nullable: false),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankAccountingCode = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierName = table.Column<string>(nullable: true),
                    IsImportSupplier = table.Column<bool>(nullable: false),
                    BGCheckNo = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DPPVATBankExpenditureNotes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DPPVATBankExpenditureNoteDetails");

            migrationBuilder.DropTable(
                name: "DPPVATBankExpenditureNoteItems");

            migrationBuilder.DropTable(
                name: "DPPVATBankExpenditureNotes");
        }
    }
}
