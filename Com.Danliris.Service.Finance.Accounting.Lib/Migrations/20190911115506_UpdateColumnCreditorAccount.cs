using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateColumnCreditorAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemoPaymentDuration",
                table: "CreditorAccounts");

            migrationBuilder.AddColumn<string>(
                name: "PaymentDuration",
                table: "CreditorAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDuration",
                table: "CreditorAccounts");

            migrationBuilder.AddColumn<int>(
                name: "MemoPaymentDuration",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
