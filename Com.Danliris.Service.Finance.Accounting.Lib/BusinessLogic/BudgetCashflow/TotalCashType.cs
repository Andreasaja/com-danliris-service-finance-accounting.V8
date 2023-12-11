namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class TotalCashType
    {
        public TotalCashType()
        {

        }

        public TotalCashType(int cashflowTypeId, CashType cashType, int currencyId, double nominal, double currencyNominal, double total)
        {
            CashflowTypeId = cashflowTypeId;
            CashType = cashType;
            CurrencyId = currencyId;
            Nominal = nominal;
            CurrencyNominal = currencyNominal;
            Total = total;
        }

        public int CashflowTypeId { get; }
        public CashType CashType { get; }
        public int CurrencyId { get; }
        public double Nominal { get; }
        public double CurrencyNominal { get; }
        public double Total { get; }
    }
}
