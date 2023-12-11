using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class DivisionTemporaryDivisionUnitDto
    {
        public DivisionTemporaryDivisionUnitDto()
        {

        }

        public DivisionTemporaryDivisionUnitDto(BudgetCashflowTypeModel cashflowType, CashType type, BudgetCashflowCategoryModel cashflowCategory, BudgetCashflowSubCategoryModel cashflowSubCategory, CurrencyDto currency, DivisionDto division)
        {
            CashflowType = cashflowType;
            Type = type;
            CashflowCategory = cashflowCategory;
            CashflowSubCategory = cashflowSubCategory;
            Currency = currency;
            Division = division;
        }

        public DivisionTemporaryDivisionUnitDto(BudgetCashflowTypeModel cashflowType, CashType type, BudgetCashflowCategoryModel cashflowCategory, BudgetCashflowSubCategoryModel cashflowSubCategory, CurrencyDto currency, DivisionDto division, UnitDto unit, BudgetCashflowUnitModel cashflowUnit)
        {
            CashflowType = cashflowType;
            Type = type;
            CashflowCategory = cashflowCategory;
            CashflowSubCategory = cashflowSubCategory;
            Currency = currency;
            Division = division;
            Unit = unit;
            CashflowUnit = cashflowUnit;
        }

        public DivisionTemporaryDivisionUnitDto(BudgetCashflowTypeModel cashflowType, CashType type, BudgetCashflowCategoryModel cashflowCategory, BudgetCashflowSubCategoryModel cashflowSubCategory, CurrencyDto currency, DivisionDto division, UnitAccountingDto unitAccounting, BudgetCashflowUnitModel cashflowUnit)
        {
            CashflowType = cashflowType;
            Type = type;
            CashflowCategory = cashflowCategory;
            CashflowSubCategory = cashflowSubCategory;
            Currency = currency;
            Division = division;
            Unit = new UnitDto {
                Code = unitAccounting.Code,
                DivisionId = unitAccounting.DivisionId,
                Id = unitAccounting.Id,
                Name = unitAccounting.Name
            };
            CashflowUnit = cashflowUnit;
        }

        public BudgetCashflowTypeModel CashflowType { get; private set; }
        public CashType Type { get; private set; }
        public BudgetCashflowCategoryModel CashflowCategory { get; private set; }
        public BudgetCashflowSubCategoryModel CashflowSubCategory { get; private set; }
        public CurrencyDto Currency { get; private set; }
        public DivisionDto Division { get; private set; }
        public UnitDto Unit { get; private set; }
        public BudgetCashflowUnitModel CashflowUnit { get; private set; }
    }
}