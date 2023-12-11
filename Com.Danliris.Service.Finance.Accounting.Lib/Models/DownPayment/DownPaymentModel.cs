using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class DownPaymentModel : StandardEntity
    {
        [MaxLength(64)]
        public string DocumentNo { get; set; }
        [MaxLength(64)]
        public string AccountName { get; set; }
        [MaxLength(64)]
        public string AccountNumber { get; set; }
        [MaxLength(64)]
        public string BankName { get; set; }
        [MaxLength(64)]
        public string CodeBankCurrency { get; set; }
        public int BuyerId { get; set; }
        [MaxLength(512)]
        public string BuyerName { get; set; }
        [MaxLength(64)]
        public string BuyerCode { get; set; }
        public int CurrencyId { get; set; }
        [MaxLength(64)]
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
        public DateTimeOffset DatePayment { get; set; }
        [MaxLength(64)]
        public string PaymentFor { get; set; }
        public string Remark { get; set; }
        public decimal TotalPayment { get; set; }
        [MaxLength(64)]
        public string CategoryAcceptance { get; set; }
    }
}