using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_BLAWBNumber_ContractPONumber_PPnAmount_PPhAmount_VBRealizationDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BLAWBNumber",
                table: "VBRealizationDocuments",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractPONumber",
                table: "VBRealizationDocuments",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BLAWBNumber",
                table: "VBRealizationDocumentExpenditureItems",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PPhAmount",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PPnAmount",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BLAWBNumber",
                table: "VBRealizationDocuments");

            migrationBuilder.DropColumn(
                name: "ContractPONumber",
                table: "VBRealizationDocuments");

            migrationBuilder.DropColumn(
                name: "BLAWBNumber",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropColumn(
                name: "PPhAmount",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropColumn(
                name: "PPnAmount",
                table: "VBRealizationDocumentExpenditureItems");
        }
    }
}
