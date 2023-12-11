using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class InvoiceFormDto
    {
        public int InvoiceId { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public double VATAmount { get; set; }
        public double IncomeTaxAmount { get; set; }
        public bool IsPayVAT { get; set; }
        public bool IsPayIncomeTax { get; set; }
        public double CurrencyVATAmount { get; set; }
        public double CurrencyIncomeTaxAmount { get; set; }
        public string VATNo { get; set; }
    }
}
