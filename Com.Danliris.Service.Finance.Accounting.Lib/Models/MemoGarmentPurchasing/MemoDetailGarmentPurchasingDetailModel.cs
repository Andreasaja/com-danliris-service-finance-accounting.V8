using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing

{
    public class MemoDetailGarmentPurchasingDetailModel : Moonlay.Models.StandardEntity
    {
        public int GarmentDeliveryOrderId { get; set; }
        public string GarmentDeliveryOrderNo { get; set; }
        public string RemarksDetail { get; set; }
        public double PaymentRate { get; set; }
        public double PurchasingRate { get; set; }
        public int MemoAmount { get; set; }
        public int MemoIdrAmount { get; set; }
        public int MemoDetailId { get; set; }
        public int MemoDispositionId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public int MemoId { get; set; }

        public string InternalNoteNo { get; set; }
        public string BillsNo { get; set; }
        public string PaymentBills { get; set; }
        public string CurrencyCode { get; set; }
        public double PurchaseAmount { get; set; }

    }
}
