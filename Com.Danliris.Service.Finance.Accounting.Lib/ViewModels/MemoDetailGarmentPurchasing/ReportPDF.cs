using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing
{
    public class ReportPDF
    {
        public int Id { get; set; }
        public int MemoId { get; set; }
        public string MemoNo { get; set; }
        public DateTimeOffset? MemoDate { get; set; }
        public string InternalNoteNo { get; set; }
        public string BillsNo { get; set; }
        public string PaymentBills { get; set; }
        public string GarmentDeliveryOrderNo { get; set; }
        public string RemarksDetail { get; set; }
        public string CurrencyCode { get; set; }
        public int MemoAmount { get; set; }
        public int MemoIdrAmount { get; set; }
        public string AccountingBookType { get; set; }
        public double PurchasingRate { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
    }
}
