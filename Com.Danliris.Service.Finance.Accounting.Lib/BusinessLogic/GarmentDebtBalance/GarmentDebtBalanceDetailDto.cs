using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class GarmentDebtBalanceDetailDto
    {
        public GarmentDebtBalanceDetailDto(int supplierId, string supplierCode, string supplierName, string billNo, string paymentBill, int deliveryOrderId, string deliveryOrderNo, string paymentType, DateTimeOffset? arrivalDate, int debtAging, int internalNoteId, string internalNoteNo, int invoiceId, string invoiceNo, double dppAmount, double currencyDPPAmount, double vatAmount, double currencyVATAmount, double incomeTaxAmount, double currencyIncomeTaxAmount, double total, double currencyTotal, int currencyId, string currencyCode, double currencyRate, string vatNo)
        {
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            BillNo = billNo;
            PaymentBill = paymentBill;
            DeliveryOrderId = deliveryOrderId;
            DeliveryOrderNo = deliveryOrderNo;
            PaymentType = paymentType;
            ArrivalDate = arrivalDate;
            DebtAging = debtAging;
            InternalNoteId = internalNoteId;
            InternalNoteNo = internalNoteNo;
            InvoiceId = invoiceId;
            InvoiceNo = invoiceNo;
            DPPAmount = dppAmount;
            CurrencyDPPAmount = currencyDPPAmount;
            VATAmount = vatAmount;
            CurrencyVATAmount = currencyVATAmount;
            IncomeTaxAmount = incomeTaxAmount;
            CurrencyIncomeTaxAmount = currencyIncomeTaxAmount;
            Total = total;
            CurrencyTotal = currencyTotal;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            CurrencyRate = currencyRate;
            VATNo = vatNo;
        }

        public int SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public string BillNo { get; private set; }
        public string PaymentBill { get; private set; }
        public int DeliveryOrderId { get; private set; }
        public string DeliveryOrderNo { get; private set; }
        public string PaymentType { get; private set; }
        public DateTimeOffset? ArrivalDate { get; private set; }
        public int DebtAging { get; private set; }
        public int InternalNoteId { get; private set; }
        public string InternalNoteNo { get; private set; }
        public int InvoiceId { get; private set; }
        public string InvoiceNo { get; private set; }
        public double DPPAmount { get; private set; }
        public double CurrencyDPPAmount { get; private set; }
        public double VATAmount { get; private set; }
        public double CurrencyVATAmount { get; private set; }
        public double IncomeTaxAmount { get; private set; }
        public double CurrencyIncomeTaxAmount { get; private set; }
        public double Total { get; private set; }
        public double CurrencyTotal { get; private set; }
        public int CurrencyId { get; private set; }
        public string CurrencyCode { get; private set; }
        public double CurrencyRate { get; private set; }
        public string VATNo { get; private set; }

        public void SetTotal()
        {
            VATNo = "TOTAL";
        }
    }
}
