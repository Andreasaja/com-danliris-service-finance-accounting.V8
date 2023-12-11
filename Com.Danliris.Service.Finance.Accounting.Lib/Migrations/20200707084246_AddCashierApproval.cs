using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddCashierApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashierAprovalVBNonPORequests",
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
                    VBNo = table.Column<string>(maxLength: 64, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    DateEstimate = table.Column<DateTimeOffset>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    UnitName = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    CurrencySymbol = table.Column<string>(maxLength: 64, nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Usage = table.Column<string>(maxLength: 255, nullable: true),
                    UnitLoad = table.Column<string>(maxLength: 255, nullable: true),
                    Apporve_Status = table.Column<bool>(nullable: false),
                    Complete_Status = table.Column<bool>(nullable: false),
                    VBRequestCategory = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierAprovalVBNonPORequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashierAprovalVBNonPORequests");
        }
    }
}
