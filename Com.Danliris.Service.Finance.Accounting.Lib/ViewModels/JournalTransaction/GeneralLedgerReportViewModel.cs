using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction
{
    public class GeneralLedgerReportViewModel
    {
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string COACode { get; set; }
        public decimal RemainingBalance { get; set; }
    }

    public class GeneralLedgerWrapperReportViewModel
    {
        public string COACode { get; set; }
        public string COAName { get; set; }
        public decimal Summary { get; set; }
        public decimal InitialBalance { get; set; }
        public List<GeneralLedgerReportViewModel> Items { get; set; }
    }
}
