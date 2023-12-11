using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddDownPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownPayments",
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
                    DocumentNo = table.Column<string>(maxLength: 64, nullable: true),
                    AccountName = table.Column<string>(maxLength: 64, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 64, nullable: true),
                    BankName = table.Column<string>(maxLength: 64, nullable: true),
                    CodeBankCurrency = table.Column<string>(maxLength: 64, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerName = table.Column<string>(maxLength: 512, nullable: true),
                    BuyerCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    DatePayment = table.Column<DateTimeOffset>(nullable: false),
                    PaymentFor = table.Column<string>(maxLength: 64, nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownPayments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownPayments");
        }
    }
}
