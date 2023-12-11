using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class GarmentDebtBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentDebtBalances",
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
                    PurchasingCategoryId = table.Column<int>(nullable: false),
                    PurchasingCategoryName = table.Column<string>(maxLength: 64, nullable: true),
                    BillsNo = table.Column<string>(maxLength: 256, nullable: true),
                    PaymentBills = table.Column<string>(maxLength: 256, nullable: true),
                    GarmentDeliveryOrderId = table.Column<int>(nullable: false),
                    GarmentDeliveryOrderNo = table.Column<string>(maxLength: 64, nullable: true),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierName = table.Column<string>(maxLength: 512, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 32, nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    InvoiceId = table.Column<int>(nullable: false),
                    InvoiceDate = table.Column<DateTimeOffset>(nullable: false),
                    InvoiceNo = table.Column<string>(maxLength: 64, nullable: true),
                    DPPAmount = table.Column<double>(nullable: false),
                    CurrencyDPPAmount = table.Column<double>(nullable: false),
                    VATAmount = table.Column<double>(nullable: false),
                    IncomeTaxAmount = table.Column<double>(nullable: false),
                    IsPayVAT = table.Column<bool>(nullable: false),
                    IsPayIncomeTax = table.Column<bool>(nullable: false),
                    InternalNoteId = table.Column<int>(nullable: false),
                    InternalNoteNo = table.Column<string>(maxLength: 64, nullable: true),
                    BankExpenditureNoteId = table.Column<int>(nullable: false),
                    BankExpenditureNoteNo = table.Column<string>(maxLength: 64, nullable: true),
                    BankExpenditureNoteInvoiceAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentDebtBalances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentDebtBalances");
        }
    }
}
