using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_Division_and_Unit_in_CreditorAccountModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                table: "CreditorAccounts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "CreditorAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "CreditorAccounts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "CreditorAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisionCode",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "CreditorAccounts");
        }
    }
}
