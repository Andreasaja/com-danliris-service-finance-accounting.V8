using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class tests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "MemorialDetailId",
            //    table: "GarmentFinanceMemorialItems");

            //migrationBuilder.DropColumn(
            //    name: "Quantity",
            //    table: "GarmentFinanceMemorialDetailItems");

            //migrationBuilder.DropColumn(
            //    name: "IsUsed",
            //    table: "GarmentFinanceBankCashReceiptDetails");

            migrationBuilder.AlterColumn<int>(
                name: "MemorialId",
                table: "GarmentFinanceMemorialItems",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IncomeType",
                table: "GarmentFinanceBankCashReceipts",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_GarmentFinanceMemorialItems_MemorialId",
            //    table: "GarmentFinanceMemorialItems",
            //    column: "MemorialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GarmentFinanceMemorialItems_MemorialId",
                table: "GarmentFinanceMemorialItems");

            migrationBuilder.AlterColumn<int>(
                name: "MemorialId",
                table: "GarmentFinanceMemorialItems",
                nullable: true,
                oldClrType: typeof(int));

            //migrationBuilder.AddColumn<int>(
            //    name: "MemorialDetailId",
            //    table: "GarmentFinanceMemorialItems",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<double>(
            //    name: "Quantity",
            //    table: "GarmentFinanceMemorialDetailItems",
            //    nullable: false,
            //    defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "IncomeType",
                table: "GarmentFinanceBankCashReceipts",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "GarmentFinanceBankCashReceiptDetails",
                nullable: false,
                defaultValue: false);
        }
    }
}
