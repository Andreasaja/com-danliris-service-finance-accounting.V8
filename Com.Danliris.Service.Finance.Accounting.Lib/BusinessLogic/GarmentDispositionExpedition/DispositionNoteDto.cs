using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition
{
    public class DispositionNoteDto
    {
        public int Id { get; set; }
        public string DocumentNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public double VATAmount { get; set; }
        public double CurrencyVATAmount { get; set; }
        public double IncomeTaxAmount { get; set; }
        public double CurrencyIncomeTaxAmount { get; set; }
        public double TotalPaid { get; set; }
        public double CurrencyTotalPaid { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public double CurrencyRate { get; set; }
        public double DPPAmount { get; set; }
        public double CurrencyDPPAmount { get; set; }
        public string ProformaNo { get; set; }
        public string Category { get; set; }
    }
}
