using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddUnitPaymentCorrectionCreditorAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPaymentCorrectionDPP",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UnitPaymentCorrectionId",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPaymentCorrectionMutation",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UnitPaymentCorrectionNo",
                table: "CreditorAccounts",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPaymentCorrectionPPN",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPaymentCorrectionDPP",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "UnitPaymentCorrectionId",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "UnitPaymentCorrectionMutation",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "UnitPaymentCorrectionNo",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "UnitPaymentCorrectionPPN",
                table: "CreditorAccounts");
        }
    }
}
