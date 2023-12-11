using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionPaymentReport
{
    public class GarmentDispositionDto
    {
        public int DispositionId { get; set; }
        public string DispositionNoteNo { get; set; }
        public DateTimeOffset DispositionNoteDate { get; set; }
        public DateTimeOffset DispositionNoteDueDate { get; set; }
        public string ProformaNo { get; set; }
        public int SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public double CurrencyRate { get; set; }
        public double DPPAmount { get; set; }
        public double VATAmount { get; set; }
        public double IncomeTaxAmount { get; set; }
        public double OthersExpenditureAmount { get; set; }
        public double TotalAmount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public int ExternalPurchaseOrderId { get; set; }
        public string ExternalPurchaseOrderNo { get; set; }
        public double DispositionQuantity { get; set; }
        public int DeliveryOrderId { get; set; }
        public string DeliveryOrderNo { get; set; }
        public double DeliveryOrderQuantity { get; set; }
        public string PaymentBillsNo { get; set; }
        public string BillsNo { get; set; }
        public int CustomsNoteId { get; set; }
        public string CustomsNoteNo { get; set; }
        public DateTimeOffset? CustomsNoteDate { get; set; }
        public int UnitReceiptNoteId { get; set; }
        public string UnitReceiptNoteNo { get; set; }
        public int InternalNoteId { get; set; }
        public string InternalNoteNo { get; set; }
        public DateTimeOffset? InternalNoteDate { get; set; }
        public string DispositionCreatedBy { get; set; }
    }
}
