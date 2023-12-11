using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionPaymentReport
{
    public class GarmentDispositionPaymentReportDto
    {
        public GarmentDispositionPaymentReportDto(int dispositionId, string dispositionNoteNo, DateTimeOffset dispositionNoteDate, DateTimeOffset dispositionNoteDueDate, string proformaNo, int supplierId, string supplierCode, string supplierName, int currencyId, string currencyCode, double currencyRate, double dPPAmount, double currencyDPPAmount, double vATAmount, double currencyVATAmount, double incomeTaxAmount, double currencyIncomeTaxAmount, double othersExpenditureAmount, double totalAmount, int categoryId, string categoryCode, string categoryName, GarmentPurchasingExpeditionPosition position, string sendToPurchasingRemark, DateTimeOffset? sendToVerificationDate, DateTimeOffset? verificationAcceptedDate, string verifiedBy, DateTimeOffset? cashierAcceptedDate, string bankExpenditureNoteDate, string bankExpenditureNoteNo, string paidAmount, int externalPurchaseOrderId, string externalPurchaseOrderNo, double dispositionQuantity, int deliveryOrderId, string deliveryOrderNo, double deliveryOrderQuantity, string paymentBillsNo, string billsNo, int customsNoteId, string customsNoteNo, DateTimeOffset? customsNoteDate, int unitReceiptNoteId, string unitReceiptNoteNo, int internalNoteId, string internalNoteNo, DateTimeOffset? internalNoteDate, string sendToVerificationBy, DateTimeOffset? verifiedDate, string remark, string purchaseBy)
        {
            DispositionId = dispositionId;
            DispositionNoteNo = dispositionNoteNo;
            DispositionNoteDate = dispositionNoteDate;
            DispositionNoteDueDate = dispositionNoteDueDate;
            ProformaNo = proformaNo;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            CurrencyRate = currencyRate;
            DPPAmount = dPPAmount;
            CurrencyDPPAmount = currencyDPPAmount;
            VATAmount = vATAmount;
            CurrencyVATAmount = currencyVATAmount;
            IncomeTaxAmount = incomeTaxAmount;
            CurrencyIncomeTaxAmount = currencyIncomeTaxAmount;
            OthersExpenditureAmount = othersExpenditureAmount;
            TotalAmount = totalAmount;
            CategoryId = categoryId;
            CategoryCode = categoryCode;
            CategoryName = categoryName;
            Position = position;
            PositionDescription = position.ToDescriptionString();
            SendToPurchasingRemark = sendToPurchasingRemark;
            SendToVerificationDate = sendToVerificationDate;
            VerificationAcceptedDate = verificationAcceptedDate;
            VerifiedBy = verifiedBy;
            VerifiedDate = verifiedDate;
            CashierAcceptedDate = cashierAcceptedDate;
            BankExpenditureNoteDate = bankExpenditureNoteDate;
            BankExpenditureNoteNo = bankExpenditureNoteNo;
            PaidAmount = paidAmount;
            ExternalPurchaseOrderId = externalPurchaseOrderId;
            ExternalPurchaseOrderNo = externalPurchaseOrderNo;
            DispositionQuantity = dispositionQuantity;
            DeliveryOrderId = deliveryOrderId;
            DeliveryOrderNo = deliveryOrderNo;
            DeliveryOrderQuantity = deliveryOrderQuantity;
            PaymentBillsNo = paymentBillsNo;
            BillsNo = billsNo;
            CustomsNoteId = customsNoteId;
            CustomsNoteNo = customsNoteNo;
            CustomsNoteDate = customsNoteDate;
            UnitReceiptNoteId = unitReceiptNoteId;
            UnitReceiptNoteNo = unitReceiptNoteNo;
            InternalNoteId = internalNoteId;
            InternalNoteNo = internalNoteNo;
            InternalNoteDate = internalNoteDate;
            SendToVerificationby = sendToVerificationBy;
            Remark = remark;
            PurchaseBy = purchaseBy;
        }

        public int DispositionId { get; private set; }
        public string DispositionNoteNo { get; private set; }
        public DateTimeOffset DispositionNoteDate { get; private set; }
        public DateTimeOffset DispositionNoteDueDate { get; private set; }
        public string ProformaNo { get; private set; }
        public int SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public int CurrencyId { get; private set; }
        public string CurrencyCode { get; private set; }
        public double CurrencyRate { get; private set; }
        public double DPPAmount { get; private set; }
        public double CurrencyDPPAmount { get; private set; }
        public double VATAmount { get; private set; }
        public double CurrencyVATAmount { get; private set; }
        public double IncomeTaxAmount { get; private set; }
        public double CurrencyIncomeTaxAmount { get; private set; }
        public double OthersExpenditureAmount { get; private set; }
        public double TotalAmount { get; private set; }
        public int CategoryId { get; private set; }
        public string CategoryCode { get; private set; }
        public string CategoryName { get; private set; }
        public GarmentPurchasingExpeditionPosition Position { get; private set; }
        public string PositionDescription { get; private set; }
        public string SendToPurchasingRemark { get; private set; }
        public DateTimeOffset? SendToVerificationDate { get; private set; }
        public DateTimeOffset? VerificationAcceptedDate { get; private set; }
        public string Remark { get; set; }
        public DateTimeOffset? VerifiedDate { get; private set; }
        public string VerifiedBy { get; private set; }
        public DateTimeOffset? CashierAcceptedDate { get; private set; }
        public string BankExpenditureNoteDate { get; private set; }
        public string BankExpenditureNoteNo { get; private set; }
        public string PaidAmount { get; private set; }
        public int ExternalPurchaseOrderId { get; private set; }
        public string ExternalPurchaseOrderNo { get; private set; }
        public double DispositionQuantity { get; private set; }
        public int DeliveryOrderId { get; private set; }
        public string DeliveryOrderNo { get; private set; }
        public double DeliveryOrderQuantity { get; private set; }
        public string PaymentBillsNo { get; private set; }
        public string BillsNo { get; private set; }
        public int CustomsNoteId { get; private set; }
        public string CustomsNoteNo { get; private set; }
        public DateTimeOffset? CustomsNoteDate { get; private set; }
        public int UnitReceiptNoteId { get; private set; }
        public string UnitReceiptNoteNo { get; private set; }
        public int InternalNoteId { get; private set; }
        public string InternalNoteNo { get; private set; }
        public DateTimeOffset? InternalNoteDate { get; private set; }
        public string SendToVerificationby { get; private set; }
        public string PurchaseBy { get; private set; }
    }
}