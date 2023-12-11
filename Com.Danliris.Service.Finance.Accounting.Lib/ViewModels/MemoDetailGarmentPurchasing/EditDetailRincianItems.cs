using System;
namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing
{
    public class EditDetailRincianItems
    {
        public int Id { get; set; }
        public int GarmentDeliveryOrderId { get; set; }
        public string GarmentDeliveryOrderNo { get; set; }
        public string RemarksDetail { get; set; }
        public int PaymentRate { get; set; }
        public int PurchasingRate { get; set; }
        public int MemoAmount { get; set; }
        public int MemoIdrAmount { get; set; }
    }
}
