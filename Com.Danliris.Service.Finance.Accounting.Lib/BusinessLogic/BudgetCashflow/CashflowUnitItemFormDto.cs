namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class CashflowUnitItemFormDto
    {
        public bool IsIDR { get; set; }
        public int CurrencyId { get; set; }
        public double Nominal { get; set; }
        public double CurrencyNominal { get; set; }
        public double Total { get; set; }
    }
}