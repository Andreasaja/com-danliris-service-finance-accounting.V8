using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote
{
    public class GarmentPurchasingPphBankExpenditureNoteItemModel : StandardEntity
    {
        public string InternalNotesNo { get; set; }
        public int InternalNotesId { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public double VAT { get; set; }
        public double CorrectionAmount { get; set; }
        public double IncomeTaxTotal { get; set; }
        public double IncomeTaxRate { get; set; }
        public int IncomeTaxId { get; set; }
        public string IncomeTaxName { get; set; }
        public double TotalPaid { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public double AmountDPP { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentDueDays { get; set; }
        public virtual int GarmentPurchasingPphBankExpenditureNoteId { get; set; }
        [ForeignKey("GarmentPurchasingPphBankExpenditureNoteId")]
        public virtual GarmentPurchasingPphBankExpenditureNoteModel GarmentPurchasingPphBankExpenditureNote { get; set; }

        public virtual ICollection<GarmentPurchasingPphBankExpenditureNoteInvoiceModel> GarmentPurchasingPphBankExpenditureNoteInvoices { get; set; }


    }
}
