using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing
{
    public class ListMemoGarmentPurchasingViewModel
    {
        public int Id { get; set; }
        public string MemoNo { get; set; }
        public DateTimeOffset? MemoDate { get; set; }
        public AccountingBookViewModel AccountingBook { get; set; }
        public GarmentCurrencyViewModel Currency { get; set; }
        public string Remarks { get; set; }
        public bool IsPosted { get; set; }
        public int TotalAmount { get; set; }
        public List<MemoGarmentPurchasingDetailViewModel> MemoGarmentPurchasingDetails { get; set; }
    }
}
