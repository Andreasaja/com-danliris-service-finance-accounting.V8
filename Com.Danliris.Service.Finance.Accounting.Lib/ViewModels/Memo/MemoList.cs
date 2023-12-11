using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo
{
    public class MemoList
    {
        public int Id { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public string DocumentNo { get; set; }
        public string SalesInvoiceNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public string MemoType { get; set; }
        public string CurrencyCodes { get; set; }
        public string BuyerName { get; set; }
    }
}
