using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.DebtorCard
{
    public class GarmentFinanceDebtorCardReportViewModel
    {
        public string Code { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? TruckingDate { get; set; }
        public DateTimeOffset? InvoiceDate { get; set; }
        public DateTimeOffset? JLDate { get; set; }
        public DateTimeOffset? BYDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceNoBY { get; set; }
        public string InvoiceNoJL { get; set; }
        public string ReceiptNo { get; set; }
        public decimal BeginAmount { get; set; }
        public decimal SellAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
