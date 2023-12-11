namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class UnitItemDto
    {
        public UnitItemDto(double total, double nominal, double currencyNominal, DivisionDto division, UnitDto unit)
        {
            Total = total;
            Nominal = nominal;
            CurrencyNominal = currencyNominal;
            Division = division;
            Unit = unit;
        }

        public double Total { get; private set; }
        public double Nominal { get; private set; }
        public double CurrencyNominal { get; private set; }
        public DivisionDto Division { get; private set; }
        public UnitDto Unit { get; private set; }
    }
}