using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels
{
    public class GarmentPurchasingInvoiceInfoDto
    {
        public string InvoiceNo { get; set; }
        public long SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public long CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public bool UseVat { get; set; }
        public bool UseIncomeTax { get; set; }
        public bool IsPayVat { get; set; }
        public bool IsPayTax { get; set; }
        public string VatNo { get; set; }
        public string IncomeTaxNo { get; set; }
        public DateTimeOffset IncomeTaxDate { get; set; }
        public long IncomeTaxId { get; set; }
        public string IncomeTaxName { get; set; }
        public double IncomeTaxRate { get; set; }
        //public double IncomeTaxRate { get; set; }
        public DateTimeOffset VatDate { get; set; }
        public bool HasInternNote { get; set; }
        public double TotalAmount { get; set; }
        public string NPN { get; set; }
        public string NPH { get; set; }
    }
}
