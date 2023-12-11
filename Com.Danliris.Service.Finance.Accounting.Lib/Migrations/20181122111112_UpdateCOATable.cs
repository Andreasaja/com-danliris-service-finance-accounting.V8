using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateCOATable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code1",
                table: "ChartsOfAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code2",
                table: "ChartsOfAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code3",
                table: "ChartsOfAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code4",
                table: "ChartsOfAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "ChartsOfAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subheader",
                table: "ChartsOfAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code1",
                table: "ChartsOfAccounts");

            migrationBuilder.DropColumn(
                name: "Code2",
                table: "ChartsOfAccounts");

            migrationBuilder.DropColumn(
                name: "Code3",
                table: "ChartsOfAccounts");

            migrationBuilder.DropColumn(
                name: "Code4",
                table: "ChartsOfAccounts");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "ChartsOfAccounts");

            migrationBuilder.DropColumn(
                name: "Subheader",
                table: "ChartsOfAccounts");
        }
    }
}
