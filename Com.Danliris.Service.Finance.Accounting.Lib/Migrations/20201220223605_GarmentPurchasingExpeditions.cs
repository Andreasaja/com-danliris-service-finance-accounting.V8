using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class GarmentPurchasingExpeditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentPurchasingExpeditions",
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
                    InternalNoteId = table.Column<int>(nullable: false),
                    InternalNoteNo = table.Column<string>(maxLength: 64, nullable: true),
                    InternalNoteDate = table.Column<DateTimeOffset>(nullable: false),
                    InternalNoteDueDate = table.Column<DateTimeOffset>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierName = table.Column<string>(maxLength: 512, nullable: true),
                    VAT = table.Column<double>(nullable: false),
                    IncomeTax = table.Column<double>(nullable: false),
                    TotalPaid = table.Column<double>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 16, nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    SendToVerificationDate = table.Column<DateTimeOffset>(nullable: true),
                    SendToVerificationBy = table.Column<string>(maxLength: 64, nullable: true),
                    VerificationAcceptedDate = table.Column<DateTimeOffset>(nullable: true),
                    VerificationAcceptedBy = table.Column<string>(maxLength: 64, nullable: true),
                    SendToCashierDate = table.Column<DateTimeOffset>(nullable: true),
                    SendToCashierBy = table.Column<string>(maxLength: 64, nullable: true),
                    CashierAcceptedDate = table.Column<DateTimeOffset>(nullable: true),
                    CashierAcceptedBy = table.Column<string>(maxLength: 64, nullable: true),
                    SendToPurchasingDate = table.Column<DateTimeOffset>(nullable: true),
                    SendToPurchasingBy = table.Column<string>(maxLength: 64, nullable: true),
                    SendToAccountingDate = table.Column<DateTimeOffset>(nullable: true),
                    SendToAccountingBy = table.Column<string>(maxLength: 64, nullable: true),
                    AccountingAcceptedDate = table.Column<DateTimeOffset>(nullable: true),
                    AccountingAcceptedBy = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentPurchasingExpeditions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentPurchasingExpeditions");
        }
    }
}
