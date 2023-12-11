using System;
namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing
{
    public class DetailRincianItems
    {
        public int Id { get; set; }
        public int GarmentDeliveryOrderId { get; set; }
        public string GarmentDeliveryOrderNo { get; set; }
        public string InternalNoteNo { get; set; }
        public string BillsNo { get; set; }
        public string PaymentBills { get; set; }
        public string SupplierCode { get; set; }
        public string RemarksDetail { get; set; }
        public string CurrencyCode { get; set; }
        public int PaymentRate { get; set; }
        public int PurchasingRate { get; set; }
        public double SaldoAkhir { get; set; }
        public int MemoAmount { get; set; }
        public int MemoIdrAmount { get; set; }
    }
}
