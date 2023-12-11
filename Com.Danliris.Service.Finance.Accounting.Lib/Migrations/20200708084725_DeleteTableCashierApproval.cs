using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class DeleteTableCashierApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashierAprovalVBNonPORequests");

            migrationBuilder.AlterColumn<bool>(
                name: "Apporve_Status",
                table: "VbRequests",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ApproveDate",
                table: "VbRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "VbRequests");

            migrationBuilder.AlterColumn<bool>(
                name: "Apporve_Status",
                table: "VbRequests",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CashierAprovalVBNonPORequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Apporve_Status = table.Column<bool>(nullable: false),
                    Complete_Status = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    CurrencySymbol = table.Column<string>(maxLength: 64, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    DateEstimate = table.Column<DateTimeOffset>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitLoad = table.Column<string>(maxLength: 255, nullable: true),
                    UnitName = table.Column<string>(maxLength: 64, nullable: true),
                    Usage = table.Column<string>(maxLength: 255, nullable: true),
                    VBNo = table.Column<string>(maxLength: 64, nullable: true),
                    VBRequestCategory = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierAprovalVBNonPORequests", x => x.Id);
                });
        }
    }
}
