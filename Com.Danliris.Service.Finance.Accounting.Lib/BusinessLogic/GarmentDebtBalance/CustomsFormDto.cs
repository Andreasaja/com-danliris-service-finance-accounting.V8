using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class CustomsFormDto
    {
        public int PurchasingCategoryId { get;  set; }
        public string PurchasingCategoryName { get;  set; }
        public string BillsNo { get;  set; }
        public string PaymentBills { get;  set; }
        public int GarmentDeliveryOrderId { get;  set; }
        public string GarmentDeliveryOrderNo { get;  set; }
        public int SupplierId { get;  set; }
        public string SupplierCode { get;  set; }
        public string SupplierName { get;  set; }
        public bool SupplierIsImport { get;  set; }
        public int CurrencyId { get;  set; }
        public string CurrencyCode { get;  set; }
        public double CurrencyRate { get;  set; }
        public string ProductNames { get; set; }
        public DateTimeOffset ArrivalDate { get; set; }
        public double DPPAmount { get; set; }
        public double CurrencyDPPAmount { get; set; }
        public string PaymentType { get; set; }
    }
}