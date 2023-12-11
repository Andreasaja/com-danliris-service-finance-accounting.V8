using System;
using Com.Moonlay.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing
{
    public class MemoDetailGarmentPurchasingModel: StandardEntity
    {
        public int MemoId { get; set; }
        public string MemoNo { get; set; }
        public DateTimeOffset MemoDate { get; set; }
        public int AccountingBookId { get; set; }
        public string AccountingBookType { get; set; }
        public int GarmentCurrenciesId { get; set; }
        public string GarmentCurrenciesCode { get; set; }
        public int GarmentCurrenciesRate { get; set; }
        public string Remarks { get; set; }
        public bool IsPosted { get; set; }
        //public ICollection<MemoDetailGarmentPurchasingDetailModel> MemoDetailGarmentPurchasingDetail { get; set; }
    }
}
