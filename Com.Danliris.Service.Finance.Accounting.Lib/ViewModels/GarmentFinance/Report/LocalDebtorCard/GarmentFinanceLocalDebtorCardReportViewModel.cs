using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalDebtorCard
{
    public class GarmentFinanceLocalDebtorCardReportViewModel
    {
        public string Code { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? SalesNoteDate { get; set; }
        public DateTimeOffset? JLDate { get; set; }
        public DateTimeOffset? BYDate { get; set; }
        public string SalesNoteNo { get; set; }
        public string SalesNoteBY { get; set; }
        public string SalesNoteNoJL { get; set; }
        public string ReceiptNo { get; set; }
        public decimal BeginAmount { get; set; }
        public decimal SellAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
