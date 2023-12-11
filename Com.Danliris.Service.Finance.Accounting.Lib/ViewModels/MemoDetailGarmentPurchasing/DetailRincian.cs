using System;
using System.Collections.Generic;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing
{
    public class DetailRincian
    {
        public int Id { get; set; }
        public int MemoId { get; set; }
        public DateTimeOffset? MemoDate { get; set; }
        public string MemoNo { get; set; }
        public int AccountingBookId { get; set; }
        public string AccountingBookType { get; set; }
        public int GarmentCurrenciesId { get; set; }
        public string GarmentCurrenciesCode { get; set; }
        public int GarmentCurrenciesRate { get; set; }
        public bool IsPosted { get; set; }
        public string Remarks { get; set; }
        public List<DetailRincianItems> Items { get; set; }
    }
}
