using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class add_currencies_in_PaymentDispositionNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "PaymentDispositionNotes",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "PaymentDispositionNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyRate",
                table: "PaymentDispositionNotes",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "PaymentDispositionNotes");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "PaymentDispositionNotes");

            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                table: "PaymentDispositionNotes");
        }
    }
}
