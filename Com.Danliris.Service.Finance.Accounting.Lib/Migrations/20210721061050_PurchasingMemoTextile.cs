using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class PurchasingMemoTextile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasingMemoTextileItems",
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
                    ChartOfAccountId = table.Column<int>(nullable: false),
                    ChartOfAccountCode = table.Column<string>(nullable: true),
                    ChartOfAccountName = table.Column<string>(nullable: true),
                    DebitAmount = table.Column<double>(nullable: false),
                    CreditAmount = table.Column<double>(nullable: false),
                    PurchasingMemoTextileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingMemoTextileItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasingMemoTextiles",
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
                    MemoDetailId = table.Column<int>(nullable: false),
                    MemoDetailDocumentNo = table.Column<string>(maxLength: 32, nullable: true),
                    MemoDetailDate = table.Column<DateTimeOffset>(nullable: false),
                    MemoDetailCurrencyId = table.Column<int>(nullable: false),
                    MemoDetailCurrencyCode = table.Column<string>(maxLength: 32, nullable: true),
                    MemoDetailCurrencyRate = table.Column<double>(nullable: false),
                    AccountingBookId = table.Column<int>(nullable: false),
                    AccountingBookType = table.Column<string>(maxLength: 256, nullable: true),
                    AccountingBookCode = table.Column<string>(maxLength: 128, nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingMemoTextiles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasingMemoTextileItems");

            migrationBuilder.DropTable(
                name: "PurchasingMemoTextiles");
        }
    }
}
