using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowUnitDto
    {
        public BudgetCashflowUnitDto(int cashflowTypeId, string cashflowTypeName, int cashflowCategoryId, string cashflowCategoryName, int cashflowSubCategoryId, string cashflowSubCategoryName, bool cashflowSubCategoryReadOnly, CashType cashflowCashType)
        {
            CashflowTypeId = cashflowTypeId;
            CashflowTypeName = cashflowTypeName;
            CashflowCategoryId = cashflowCategoryId;
            CashflowCategoryName = cashflowCategoryName;
            CashflowSubCategoryId = cashflowSubCategoryId;
            CashflowSubCategoryName = cashflowSubCategoryName;
            IsReadOnly = cashflowSubCategoryReadOnly;
            TypeName = cashflowCashType.ToDescriptionString();
        }

        public BudgetCashflowUnitDto(BudgetCashflowUnitDto cashflowItem)
        {
            CashflowTypeId = cashflowItem.CashflowTypeId;
            CashflowTypeName = cashflowItem.CashflowTypeName;
            CashflowCategoryId = cashflowItem.CashflowCategoryId;
            CashflowCategoryName = cashflowItem.CashflowCategoryName;
            CashflowSubCategoryId = cashflowItem.CashflowSubCategoryId;
            CashflowSubCategoryName = cashflowItem.CashflowSubCategoryName;
            TypeName = cashflowItem.TypeName;
            IsReadOnly = cashflowItem.IsReadOnly;
        }

        public BudgetCashflowUnitDto(BudgetCashflowUnitDto cashflowItem, bool isFirst) : this(cashflowItem)
        {
            IsShowLabel = isFirst;
        }

        public BudgetCashflowUnitDto(int cashflowTypeId, string cashflowTypeName)
        {
            CashflowTypeId = cashflowTypeId;
            CashflowTypeName = cashflowTypeName;
        }

        public BudgetCashflowUnitDto(BudgetCashflowTypeModel cashflowType, BudgetCashflowCategoryModel cashflowCategory, BudgetCashflowSubCategoryModel cashflowSubCategory, BudgetCashflowUnitModel cashflowUnit)
        {
            CashflowType = cashflowType;
            CashflowCategory = cashflowCategory;
            CashflowSubCategory = cashflowSubCategory;
            CashflowUnit = cashflowUnit;
        }

        public int CashflowTypeId { get; private set; }
        public string CashflowTypeName { get; private set; }
        public int CashflowCategoryId { get; private set; }
        public string CashflowCategoryName { get; private set; }
        public int CashflowSubCategoryId { get; private set; }
        public string CashflowSubCategoryName { get; private set; }
        public bool IsReadOnly { get; private set; }
        public string TypeName { get; private set; }
        public bool IsUseSection { get; private set; }
        public int GroupRowspan { get; private set; }
        public bool IsUseGroup { get; private set; }
        public bool IsLabelOnly { get; private set; }
        public CurrencyDto Currency { get; private set; }
        public double CurrencyNominal { get; private set; }
        public double Nominal { get; private set; }
        public double Total { get; private set; }
        public bool IsecondLevel { get; private set; }
        public int CashflowTypeRowspan { get; private set; }
        public bool IsShowLabel { get; private set; }
        public BudgetCashflowTypeModel CashflowType { get; private set; }
        public BudgetCashflowCategoryModel CashflowCategory { get; private set; }
        public BudgetCashflowSubCategoryModel CashflowSubCategory { get; private set; }
        public BudgetCashflowUnitModel CashflowUnit { get; private set; }

        public void UseSection()
        {
            IsUseSection = true;
        }

        public void UseGroup()
        {
            IsUseGroup = true;
        }

        public void LabelOnly()
        {
            IsLabelOnly = true;
        }

        public void NotLabelOnly()
        {
            IsLabelOnly = false;
        }

        internal void SetNominal(CurrencyDto currency, double currencyNominal, double nominal, double total)
        {
            Currency = currency;
            CurrencyNominal = currencyNominal;
            Nominal = nominal;
            Total = total;
            IsecondLevel = true;
        }

        public void SetCashflowTypeRowspan(int cashflowTypeRowspan)
        {
            CashflowTypeRowspan = cashflowTypeRowspan;
            IsLabelOnly = true;
            IsReadOnly = true;
        }

        public void SetGroupRowspan(int rowspan)
        {
            GroupRowspan = rowspan;
            IsUseGroup = true;
        }

        public void ShowLabel()
        {
            IsShowLabel = true;
        }
    }
}