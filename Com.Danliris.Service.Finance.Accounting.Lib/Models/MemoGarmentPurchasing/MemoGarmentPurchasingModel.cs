using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing
{
    public class MemoGarmentPurchasingModel : StandardEntity
    {
        public string? MemoNo { get; set; }
        public DateTimeOffset MemoDate { get; set; }
        public int AccountingBookId { get; set; }
        public string? AccountingBookCode { get; set; }
        public string? AccountingBookType { get; set; }
        public int GarmentCurrenciesId { get; set; }
        public string? GarmentCurrenciesCode { get; set; }
        public int GarmentCurrenciesRate { get; set; }
        public double TotalAmount { get; set; }
        public string? Remarks { get; set; }
        public bool IsPosted { get; set; }
        public ICollection<MemoGarmentPurchasingDetailModel> MemoGarmentPurchasingDetails { get; set; }
    }
}
