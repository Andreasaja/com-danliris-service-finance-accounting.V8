using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetailLocal
{
    public class GarmentFinanceBankCashReceiptDetailLocalItemModel : StandardEntity
    {
        public int LocalSalesNoteId { get; set; }
        public string? LocalSalesNoteNo { get; set; }

        public int BuyerId { get; set; }
        [MaxLength(255)]
        public string? BuyerCode { get; set; }
        [MaxLength(1000)]
        public string? BuyerName { get; set; }

        public int CurrencyId { get; set; }
        [MaxLength(32)]
        public string? CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }

        public decimal Amount { get; set; }

        public virtual int BankCashReceiptDetailLocalId { get; set; }
        [ForeignKey("BankCashReceiptDetailLocalId")]
        public virtual GarmentFinanceBankCashReceiptDetailLocalModel GarmentFinanceBankCashReceiptDetailLocalModel { get; set; }
    }
}
