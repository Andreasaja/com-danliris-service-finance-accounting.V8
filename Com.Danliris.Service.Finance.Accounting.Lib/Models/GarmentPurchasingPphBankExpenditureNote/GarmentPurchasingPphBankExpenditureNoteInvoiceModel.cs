using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote
{
    public class GarmentPurchasingPphBankExpenditureNoteInvoiceModel: StandardEntity
    {
        public string? InvoicesNo { get; set; }
        public DateTimeOffset InvoicesDate { get; set; }
        public long InvoicesId { get; set; }
        public string? ProductName { get; set; }
        public long ProductId { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductCode { get; set; }
        public decimal Total { get; set; }
        public string? UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? UnitCode { get; set; }
        public string? PaymentBill { get; set; }
        public string? BillNo { get; set; }
        public string? DoNo { get; set; }
        public string? NPH { get; set; }
        public virtual int GarmentPurchasingPphBankExpenditureNoteItemId { get; set; }
        [ForeignKey("GarmentPurchasingPphBankExpenditureNoteItemId")]
        public virtual GarmentPurchasingPphBankExpenditureNoteItemModel GarmentPurchasingPphBankExpenditureNoteItem { get; set; }
    }
}
