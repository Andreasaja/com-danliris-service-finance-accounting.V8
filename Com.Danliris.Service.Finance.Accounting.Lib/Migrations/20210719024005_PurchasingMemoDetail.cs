using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class PurchasingMemoDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasingMemoDetailTextileDetails",
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
                    PurchasingMemoDetailTextileId = table.Column<int>(nullable: false),
                    PurchasingMemoDetailTextileItemId = table.Column<int>(nullable: false),
                    ExpenditureId = table.Column<int>(nullable: false),
                    ExpenditureNo = table.Column<string>(maxLength: 64, nullable: true),
                    ExpenditureDate = table.Column<DateTimeOffset>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierCode = table.Column<string>(maxLength: 64, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 512, nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    UnitPaymentOrderId = table.Column<int>(nullable: false),
                    UnitPaymentOrderNo = table.Column<string>(maxLength: 64, nullable: true),
                    UnitPaymentOrderDate = table.Column<DateTimeOffset>(nullable: false),
                    PurchaseAmountCurrency = table.Column<double>(nullable: false),
                    PurchaseAmount = table.Column<double>(nullable: false),
                    PaymentAmountCurrency = table.Column<double>(nullable: false),
                    PaymentAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingMemoDetailTextileDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasingMemoDetailTextileItems",
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
                    DispositionId = table.Column<int>(nullable: false),
                    DispositionNo = table.Column<string>(maxLength: 64, nullable: true),
                    DispositionDate = table.Column<DateTimeOffset>(nullable: false),
                    PurchasingMemoDetailTextileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingMemoDetailTextileItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasingMemoDetailTextiles",
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
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    DivisionId = table.Column<int>(nullable: false),
                    DivisionCode = table.Column<string>(maxLength: 64, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 128, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    SupplierIsImport = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    DocumentNo = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingMemoDetailTextiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasingMemoDetailTextileUnitReceiptNotes",
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
                    PurchasingMemoDetailTextileId = table.Column<int>(nullable: false),
                    PurchasingMemoDetailTextileItemId = table.Column<int>(nullable: false),
                    PurchasingMemoDetailTextileDetailId = table.Column<int>(nullable: false),
                    UnitReceiptNoteId = table.Column<int>(nullable: false),
                    UnitReceiptNoteNo = table.Column<string>(maxLength: 64, nullable: true),
                    UnitReceiptNoteDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingMemoDetailTextileUnitReceiptNotes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasingMemoDetailTextileDetails");

            migrationBuilder.DropTable(
                name: "PurchasingMemoDetailTextileItems");

            migrationBuilder.DropTable(
                name: "PurchasingMemoDetailTextiles");

            migrationBuilder.DropTable(
                name: "PurchasingMemoDetailTextileUnitReceiptNotes");
        }
    }
}
