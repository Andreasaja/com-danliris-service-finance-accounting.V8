using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow
{
    public class RealCashBalanceModel : StandardEntity
    {
        public RealCashBalanceModel()
        {

        }

        public RealCashBalanceModel(int unitId, int divisionId, int currencyId, double nominal, double currencyNominal, double total, int month, int year)
        {
            UnitId = unitId;
            DivisionId = divisionId;
            CurrencyId = currencyId;
            Nominal = nominal;
            CurrencyNominal = currencyNominal;
            Total = total;
            Month = month;
            Year = year;
        }

        public int UnitId { get; private set; }
        public int DivisionId { get; private set; }
        public int CurrencyId { get; private set; }
        public double Nominal { get; private set; }
        public double CurrencyNominal { get; private set; }
        public double Total { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
    }
}
