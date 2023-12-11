using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow
{
    public class BudgetCashflowUnitModel : StandardEntity
    {
        public BudgetCashflowUnitModel()
        {

        }

        public BudgetCashflowUnitModel(int budgetCashflowSubCategoryId, int unitId, int divisionId, DateTimeOffset date, int currencyId, double currencyNominal, double nominal, double total)
        {
            BudgetCashflowSubCategoryId = budgetCashflowSubCategoryId;
            UnitId = unitId;
            DivisionId = divisionId;
            CurrencyId = currencyId;
            CurrencyNominal = currencyNominal;
            Nominal = nominal;
            Total = total;
            Month = date.Month;
            Year = date.Year;
        }

        public int BudgetCashflowSubCategoryId { get; private set; }
        public int UnitId { get; private set; }
        public int DivisionId { get; private set; }
        public int CurrencyId { get; private set; }
        public double CurrencyNominal { get; private set; }
        public double Nominal { get; private set; }
        public double Total { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
    }
}
