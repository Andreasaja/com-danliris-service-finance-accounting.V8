using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt
{
    public class SalesInvoiceReportSalesReceiptViewModel
    {
        public int SalesInvoiceId { get; set; }
        public string SalesReceiptNo { get; set; }
        public DateTimeOffset SalesReceiptDate { get; set; }
        public double Nominal { get; set; }
        public double TotalPaid { get; set; }
        public double UnPaid { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
