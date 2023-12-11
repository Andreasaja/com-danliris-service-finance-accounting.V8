using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction
{
    public class JournalTransactionReportHeaderViewModel
    {
        public string ReferenceNo { get; set; }

        public string Description { get; set; }
        public string HeaderRemark { get; set; }

        public List<JournalTransactionReportViewModel> Items { get; set; }
        public string Remark { get; internal set; }
    }
}
