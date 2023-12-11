using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class addingDetailDoDPPVatBankExpenditure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailSJ",
                table: "DPPVATBankExpenditureNoteDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DPPVATBankExpenditureNoteDetailDos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DONo = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<double>(nullable: false),
                    PaymentBill = table.Column<string>(nullable: true),
                    BillNo = table.Column<string>(nullable: true),
                    DOId = table.Column<long>(nullable: false),
                    CurrencyRate = table.Column<double>(nullable: false),
                    DPPVATBankExpenditureNoteId = table.Column<int>(nullable: false),
                    DPPVATBankExpenditureNoteItemId = table.Column<int>(nullable: false),
                    DPPVATBankExpenditureNoteDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DPPVATBankExpenditureNoteDetailDos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DPPVATBankExpenditureNoteDetailDos_DPPVATBankExpenditureNoteDetails_DPPVATBankExpenditureNoteDetailId",
                        column: x => x.DPPVATBankExpenditureNoteDetailId,
                        principalTable: "DPPVATBankExpenditureNoteDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DPPVATBankExpenditureNoteDetailDos_DPPVATBankExpenditureNoteDetailId",
                table: "DPPVATBankExpenditureNoteDetailDos",
                column: "DPPVATBankExpenditureNoteDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DPPVATBankExpenditureNoteDetailDos");

            migrationBuilder.DropColumn(
                name: "DetailSJ",
                table: "DPPVATBankExpenditureNoteDetails");
        }
    }
}
