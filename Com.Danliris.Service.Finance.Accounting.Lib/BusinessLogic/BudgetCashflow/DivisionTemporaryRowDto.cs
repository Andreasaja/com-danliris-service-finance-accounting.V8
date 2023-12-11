using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class DivisionTemporaryRowDto
    {
        public DivisionTemporaryRowDto()
        {
            Items = new List<DivisionTemporaryDivisionUnitDto>();
        }

        public DivisionTemporaryRowDto(BudgetCashflowTypeModel cashflowType, CashType type, BudgetCashflowCategoryModel cashflowCategory) : this()
        {
            CashflowType = cashflowType;
            Type = type;
            CashflowCategory = cashflowCategory;
        }

        public DivisionTemporaryRowDto(BudgetCashflowTypeModel cashflowType, CashType type, BudgetCashflowCategoryModel cashflowCategory, BudgetCashflowSubCategoryModel cashflowSubCategory, CurrencyDto currency) : this(cashflowType, type, cashflowCategory)
        {
            Currency = currency;
            CashflowSubCategory = cashflowSubCategory;
        }

        public BudgetCashflowCategoryModel CashflowCategory { get; private set; }
        public CurrencyDto Currency { get; private set; }
        public BudgetCashflowSubCategoryModel CashflowSubCategory { get; private set; }
        public List<DivisionTemporaryDivisionUnitDto> Items { get; private set; }
        public BudgetCashflowTypeModel CashflowType { get; private set; }
        public CashType Type { get; private set; }
    }
}