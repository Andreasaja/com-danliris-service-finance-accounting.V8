using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail
{
    public class BankCashReceiptDetailItemModel : StandardEntity
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }

        public int BuyerId { get; set; }
        [MaxLength(255)]
        public string BuyerCode { get; set; }
        [MaxLength(1000)]
        public string BuyerName { get; set; }

        public int CurrencyId { get; set; }
        [MaxLength(32)]
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }

        public decimal Amount { get; set; }

        public virtual int BankCashReceiptDetailId { get; set; }
        [ForeignKey("BankCashReceiptDetailId")]
        public virtual BankCashReceiptDetailModel BankCashReceiptDetailModel { get; set; }
    }
}
