using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDownPaymentReport
{
    public class MemoDocumentDto
    {
        public MemoDocumentDto(int memoId, string memoNo, DateTimeOffset memoDate, double realizationDownPaymentCurrencyTotal, double realizationDownPaymentCurrencyRate, double realizationDownPaymentCurrencyAmount, DateTimeOffset internNoteDate, string internNoteNo, DateTimeOffset deliveryOrderDate, string deliveryOrderNo, string paymentNo, string paymentDescription, string paymentCurrencyCode, double paymentCurrencyRate, double paymentCurrencyAmount, double differenceCurrencyRate)
        {
            MemoId = memoId;
            MemoNo = memoNo;
            MemoDate = memoDate;
            RealizationDownPaymentCurrencyTotal = realizationDownPaymentCurrencyTotal;
            RealizationDownPaymentCurrencyRate = realizationDownPaymentCurrencyRate;
            RealizationDownPaymentCurrencyAmount = realizationDownPaymentCurrencyAmount;
            InternNoteDate = internNoteDate;
            InternNoteNo = internNoteNo;
            DeliveryOrderDate = deliveryOrderDate;
            DeliveryOrderNo = deliveryOrderNo;
            PaymentNo = paymentNo;
            PaymentDescription = paymentDescription;
            PaymentCurrencyCode = paymentCurrencyCode;
            PaymentCurrencyRate = paymentCurrencyRate;
            PaymentCurrencyAmount = paymentCurrencyAmount;
            DifferenceCurrencyRate = differenceCurrencyRate;
        }

        public int MemoId { get; private set; }
        public string MemoNo { get; private set; }
        public DateTimeOffset MemoDate { get; private set; }
        public double RealizationDownPaymentCurrencyTotal { get; set; }
        public double RealizationDownPaymentCurrencyRate { get; set; }
        public double RealizationDownPaymentCurrencyAmount { get; set; }
        public DateTimeOffset InternNoteDate { get; set; }
        public string InternNoteNo { get; set; }
        public DateTimeOffset DeliveryOrderDate { get; set; }
        public string DeliveryOrderNo { get; set; }
        public string PaymentNo { get; set; }
        public string PaymentDescription { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public double PaymentCurrencyRate { get; set; }
        public double PaymentCurrencyAmount { get; set; }
        public double DifferenceCurrencyRate { get; set; }

    }
}