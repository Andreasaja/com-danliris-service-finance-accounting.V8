using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction
{
    public class SubLedgerReportViewModel
    {
        public SubLedgerReportViewModel()
        {
            TextileLokals = new List<SubLedgerReport>();
            TextileImports = new List<SubLedgerReport>();
            GarmentLokals = new List<SubLedgerReport>();
            GarmentImports = new List<SubLedgerReport>();
            Others = new List<SubLedgerReport>();
        }
        public decimal InitialBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        //public List<SubLedgerReport> Info { get; set; }
        public List<SubLedgerReport> TextileLokals { get; set; }
        public List<SubLedgerReport> TextileImports { get; set; }
        public List<SubLedgerReport> GarmentLokals { get; set; }
        public List<SubLedgerReport> GarmentImports { get; set; }
        public List<SubLedgerReport> Others { get; set; }
    }

    public class SubLedgerReport
    {
        public string Date { get; set; }
        public string Remark { get; set; }
        public string Supplier { get; set; }
        public string URNNo { get; set; }
        public string UPONo { get; set; }
        public string COAName { get; set; }
        public string COACode { get; set; }

        public string No { get; set; }
        public string BankName { get; set; }
        public string BGCheck { get; set; }

        public int JournalId { get; set; }
        public int JournalItemId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        //sorting by date purpose only
        public DateTimeOffset SortingDate { get; set; }
    }

    public class SubLedgerXlsFormat
    {
        public string Filename { get; set; }
        public MemoryStream Result { get; set; }
    }
    
}
