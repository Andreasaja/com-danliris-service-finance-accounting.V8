using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.OthersExpenditureProofDocument
{
    public class OthersExpenditureProofDocumentReportListViewModel
    {
        public int Total { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public List<OthersExpenditureProofDocumentReportViewModel> Data { get; set; }
    }
}
