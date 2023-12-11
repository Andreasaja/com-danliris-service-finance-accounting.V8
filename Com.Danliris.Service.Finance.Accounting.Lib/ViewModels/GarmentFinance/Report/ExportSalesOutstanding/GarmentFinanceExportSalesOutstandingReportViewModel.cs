using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.ExportSalesOutstanding
{
    public class GarmentFinanceExportSalesOutstandingReportViewModel
    {
        public int Index { get; set; }
        public DateTimeOffset? TruckingDate { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public int InvoiceId { get; set; }
        public string BuyerName { get; set; }
    }
}
