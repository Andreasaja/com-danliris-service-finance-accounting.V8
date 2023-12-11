using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt
{
    public class SalesReceiptReportViewModel
    {
        public string SalesReceiptNo { get; set; }
        public DateTimeOffset SalesReceiptDate { get; set; }
        public double TotalPaid { get; set; }
        public string CurrencyCode { get; set; }
        public string Buyer { get; set; }
    }
}
