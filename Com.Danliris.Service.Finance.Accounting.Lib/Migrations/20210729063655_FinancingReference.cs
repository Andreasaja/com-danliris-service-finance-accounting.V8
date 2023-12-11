using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class FinancingReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinancingSourceReferenceId",
                table: "DailyBankTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FinancingSourceReferenceNo",
                table: "DailyBankTransactions",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancingSourceReferenceId",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "FinancingSourceReferenceNo",
                table: "DailyBankTransactions");
        }
    }
}
