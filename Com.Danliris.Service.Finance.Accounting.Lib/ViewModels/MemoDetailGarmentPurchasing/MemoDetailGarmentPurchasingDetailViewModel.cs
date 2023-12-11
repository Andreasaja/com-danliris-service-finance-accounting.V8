using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing
{
    public class MemoDetailGarmentPurchasingDetailViewModel
    {
        public int GarmentDeliveryOrderId { get; set; }
        public string GarmentDeliveryOrderNo { get; set; }
        public string RemarksDetail { get; set; }
        public int PaymentRate { get; set; }
        public int PurchasingRate { get; set; }
        public int MemoAmount { get; set; }
        public int MemoIdrAmount { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
    }
}
