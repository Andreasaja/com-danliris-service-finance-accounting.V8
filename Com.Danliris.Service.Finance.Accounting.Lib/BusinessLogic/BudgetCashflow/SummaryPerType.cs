using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class SummaryPerType
    {
        public SummaryPerType()
        {
            CashflowCategories = new List<BudgetCashflowCategoryModel>();
            Items = new List<BudgetCashflowUnitDto>();
            TotalCashTypes = new List<TotalCashType>();
        }

        public SummaryPerType(int cashflowTypeId, List<BudgetCashflowUnitDto> cashIn, List<BudgetCashflowUnitDto> cashOut, List<BudgetCashflowUnitDto> totalCashIn, List<BudgetCashflowUnitDto> totalCashOut)
        {

        }

        public BudgetCashflowTypeModel CashflowType { get; private set; }
        public List<BudgetCashflowCategoryModel> CashflowCategories { get; }
        public List<BudgetCashflowUnitDto> Items { get; }
        public List<BudgetCashflowDivisionDto> DivisionItems { get; }
        public List<TotalCashType> TotalCashTypes { get; }

        public void SetCashflowType(BudgetCashflowTypeModel cashflowType)
        {
            CashflowType = cashflowType;
        }

        public void AddTotalCashTypes(List<TotalCashType> totalCashTypes)
        {
            TotalCashTypes.AddRange(totalCashTypes);
        }

        public List<TotalCashType> GetDifference(int cashflowTypeId)
        {
            return TotalCashTypes
                .Where(element => element.CashflowTypeId == cashflowTypeId)
                .GroupBy(element => new { element.CashflowTypeId, element.CurrencyId })
                .Select(element =>
                {
                    var nominalIn = element.Where(w => w.CashType == CashType.In).Sum(sum => sum.Nominal);
                    var nominalOut = element.Where(w => w.CashType == CashType.Out).Sum(sum => sum.Nominal);

                    var currencyNominalIn = element.Where(w => w.CashType == CashType.In).Sum(sum => sum.CurrencyNominal);
                    var currencyNominalOut = element.Where(w => w.CashType == CashType.Out).Sum(sum => sum.CurrencyNominal);

                    var totalIn = element.Where(w => w.CashType == CashType.In).Sum(sum => sum.Total);
                    var totalOut = element.Where(w => w.CashType == CashType.Out).Sum(sum => sum.Total);

                    return new TotalCashType(cashflowTypeId, 0, element.Key.CurrencyId, nominalIn - nominalOut, currencyNominalIn - currencyNominalOut, totalIn - totalOut);
                })
                .ToList();
        }
    }
}
