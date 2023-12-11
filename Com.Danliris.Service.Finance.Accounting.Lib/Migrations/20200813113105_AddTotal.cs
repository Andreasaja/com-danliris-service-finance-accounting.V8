using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IncomeTaxRate",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IncomeTaxId",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "RealizationVbDetails",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "RealizationVbDetails");

            migrationBuilder.AlterColumn<string>(
                name: "IncomeTaxRate",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(double),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "IncomeTaxId",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 64);
        }
    }
}
