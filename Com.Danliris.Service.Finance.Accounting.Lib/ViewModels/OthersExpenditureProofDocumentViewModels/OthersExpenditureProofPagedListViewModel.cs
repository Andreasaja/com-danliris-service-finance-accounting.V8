using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels
{
    public class OthersExpenditureProofPagedListViewModel
    {
        public int Total { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public List<OthersExpenditureProofListViewModel> Data { get; set; }
    }

    public class OthersExpenditureProofListViewModel
    {
        public int Id { get; set; }
        public string DocumentNo { get; set; }
        public int AccountBankId { get; set; }
        public decimal Total { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Type { get; set; }
        public bool IsPosted { get; internal set; }
    }
}
