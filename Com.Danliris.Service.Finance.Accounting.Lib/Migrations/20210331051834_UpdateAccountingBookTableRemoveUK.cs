using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateAccountingBookTableRemoveUK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountingBooks_AccountingBookType",
                table: "AccountingBooks");

            migrationBuilder.DropIndex(
                name: "IX_AccountingBooks_Code",
                table: "AccountingBooks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccountingBooks_AccountingBookType",
                table: "AccountingBooks",
                column: "AccountingBookType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountingBooks_Code",
                table: "AccountingBooks",
                column: "Code",
                unique: true);
        }
    }
}
