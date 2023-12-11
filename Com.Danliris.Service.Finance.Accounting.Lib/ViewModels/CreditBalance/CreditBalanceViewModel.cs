namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditBalance
{
    public class CreditBalanceViewModel
    {
        public string Currency { get; set; }

        public string Products { get; set; }

        public string SupplierName { get; set; }

        public decimal StartBalance { get; set; }
        public decimal StartBalanceCurrency { get; set; }

        public decimal Purchase { get; set; }
        public decimal PurchaseCurrency { get; set; }

        public decimal Payment { get; set; }
        public decimal PaymentCurrency { get; set; }
        public double PaidAmount { get; set; }
        public double PaidAmountCurrency { get; set; }

        public decimal FinalBalance { get; set; }
        public decimal FinalBalanceCurrency { get; set; }

        public decimal CurrencyRate { get; set; }

        public string DivisionName { get; set; }
        public int DivisionId { get; set; }
        public string SupplierCode { get; set; }
    }
}
