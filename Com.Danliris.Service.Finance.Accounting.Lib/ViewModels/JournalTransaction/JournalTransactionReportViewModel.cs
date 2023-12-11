using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction
{
    public class JournalTransactionReportViewModel
    {
        public DateTimeOffset Date { get; set; }

        public string COAName { get; set; }

        public string COACode { get; set; }

        public string Remark { get; set; }
        public string HeaderRemark { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public string ReferenceNo { get; set; }

        public string Description { get; set; }
        public bool IsReverser { get; set; }
        public bool IsReversed { get; set; }
    }
}
