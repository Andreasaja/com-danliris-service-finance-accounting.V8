namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing
{
    public class MemoDetail
    {
        public int GarmentDeliveryOrderId { get; set; }
        public string GarmentDeliveryOrderNo { get; set; }
        public string RemarksDetail { get; set; }
        public double PurchasingRate { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string InternalNoteNo { get; set; }
        public string BillsNo { get; set; }
        public string PaymentBills { get; set; }
        public string CurrencyCode { get; set; }
        public double PurchaseAmount { get; set; }

        public double PaymentRate { get; set; }
        public double MemoAmount { get; set; }
        public double MemoIdrAmount { get; set; }
    }
}