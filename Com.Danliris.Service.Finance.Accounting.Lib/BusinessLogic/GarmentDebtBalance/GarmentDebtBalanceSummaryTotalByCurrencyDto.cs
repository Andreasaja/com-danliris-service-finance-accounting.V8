namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class GarmentDebtBalanceSummaryTotalByCurrencyDto
    {
        public string CurrencyCode { get; set; }
        public double TotalCurrencyInitialBalance { get; set; }
        public double TotalCurrencyPurchase { get; set; }
        public double TotalCurrencyPayment { get; set; }
        public double TotalCurrencyCurrentBalance { get; set; }

        public double TotalInitialBalance { get; set; }
        public double TotalPurchase { get; set; }
        public double TotalPayment { get; set; }
        public double TotalCurrentBalance { get; set; }

    }
}