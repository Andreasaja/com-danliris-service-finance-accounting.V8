using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class DownPaymentList
    {
        public int Id { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public string DocumentNo { get; set; }
        public string BuyerName { get; set; }
        public DateTimeOffset DatePayment { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TotalPayment { get; set; }
        public string CategoryAcceptance { get; set; }
    }
}