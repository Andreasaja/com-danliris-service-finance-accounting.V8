using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail
{
    public class BankCashReceiptDetailModel : StandardEntity
    {
        public int BankCashReceiptId { get; set; }
        [MaxLength(256)]
        public string? BankCashReceiptNo { get; set; }
        public DateTimeOffset BankCashReceiptDate { get; set; }
        public decimal Amount { get; set; }
        public int DebitCoaId { get; set; }
        [MaxLength(32)]
        public string? DebitCoaCode { get; set; }
        [MaxLength(256)]
        public string? DebitCoaName { get; set; }
        public int InvoiceCoaId { get; set; }
        [MaxLength(32)]
        public string? InvoiceCoaCode { get; set; }
        [MaxLength(256)]
        public string? InvoiceCoaName { get; set; }
        public virtual ICollection<BankCashReceiptDetailItemModel> Items { get; set; }
        public virtual ICollection<BankCashReceiptDetailOtherItemModel> OtherItems { get; set; }
    }
}
