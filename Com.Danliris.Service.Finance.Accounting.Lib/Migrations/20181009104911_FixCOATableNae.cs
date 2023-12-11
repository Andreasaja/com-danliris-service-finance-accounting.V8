using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class FixCOATableNae : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_COAs",
                table: "COAs");

            migrationBuilder.RenameTable(
                name: "COAs",
                newName: "ChartsOfAccounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChartsOfAccounts",
                table: "ChartsOfAccounts",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChartsOfAccounts",
                table: "ChartsOfAccounts");

            migrationBuilder.RenameTable(
                name: "ChartsOfAccounts",
                newName: "COAs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_COAs",
                table: "COAs",
                column: "Id");
        }
    }
}
