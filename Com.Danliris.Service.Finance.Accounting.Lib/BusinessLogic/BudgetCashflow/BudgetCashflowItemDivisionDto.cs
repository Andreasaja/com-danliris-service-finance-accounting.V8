using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using System.Collections.Generic;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowItemDivisionDto
    {
        public BudgetCashflowItemDivisionDto()
        {

        }

        public BudgetCashflowItemDivisionDto(int cashflowTypeId, string cashflowTypeName, bool isUseSection)
        {
            CashflowTypeId = cashflowTypeId;
            CashflowTypeName = cashflowTypeName;
            IsUseSection = isUseSection;
        }

        public BudgetCashflowItemDivisionDto(BudgetCashflowCategoryModel cashflowCategory)
        {
            IsLabelOnly = true;
            CashflowCategoryId = cashflowCategory.Id;
            CashflowCategoryName = cashflowCategory.Name;
        }

        public BudgetCashflowItemDivisionDto(bool isShowSubCategoryLabel, BudgetCashflowUnitDto item, List<CurrencyDto> currencies)
        {
            var currency = currencies.FirstOrDefault(element => element.Id == item.CashflowUnit.CurrencyId);
            IsShowSubCategoryLabel = isShowSubCategoryLabel;
            SubCategoryId = item.CashflowSubCategory.Id;
            SubCategoryName = item.CashflowSubCategory.Name;
            Currency = currency;
            //Nominal = item.CashflowUnit.Nominal;
            //CurrencyNominal = item.CashflowUnit.CurrencyNominal;
            //Total = item.CashflowUnit.Total;
        }

        public BudgetCashflowItemDivisionDto(bool isShowTotalLabel, string totalLabel, TotalCashType totalCashType, List<CurrencyDto> currencies)
        {
            var currency = currencies.FirstOrDefault(element => element.Id == totalCashType.CurrencyId);
            IsShowTotalLabel = isShowTotalLabel;
            TotalLabel = totalLabel;
            Currency = currency;
            //Nominal = totalCashType.Nominal;
            //CurrencyNominal = totalCashType.CurrencyNominal;
            //Total = totalCashType.Total;
        }

        public BudgetCashflowItemDivisionDto(bool isShowDifferenceLabel, string differenceLabel, TotalCashType differenceCashType, List<CurrencyDto> currencies, bool isShowDifference)
        {
            var currency = currencies.FirstOrDefault(element => element.Id == differenceCashType.CurrencyId);
            IsShowDifferenceLabel = isShowDifferenceLabel;
            DifferenceLabel = differenceLabel;
            Currency = currency;
            //Nominal = differenceCashType.Nominal;
            //CurrencyNominal = differenceCashType.CurrencyNominal;
            //Total = differenceCashType.Total;
            IsShowDifference = isShowDifference;
        }

        public int CashflowTypeId { get; private set; }
        public string CashflowTypeName { get; private set; }
        public bool IsUseSection { get; private set; }
        public int SectionRowSpan { get; private set; }
        public bool IsUseGroup { get; private set; }
        public int GroupRowSpan { get; private set; }
        public CashType Type { get; private set; }
        public string TypeName { get; private set; }
        public bool IsLabelOnly { get; private set; }
        public int CashflowCategoryId { get; private set; }
        public string CashflowCategoryName { get; private set; }
        public bool IsShowSubCategoryLabel { get; private set; }
        public int SubCategoryId { get; private set; }
        public string SubCategoryName { get; private set; }
        public CurrencyDto Currency { get; private set; }
        public bool IsShowDifference { get; private set; }
        public bool IsShowTotalLabel { get; private set; }
        public string TotalLabel { get; private set; }
        public bool IsShowDifferenceLabel { get; private set; }
        public string DifferenceLabel { get; private set; }

        public List<UnitItemDto> UnitItems { get; private set; }

        public void SetSectionRowSpan(int sectionRowSpan)
        {
            SectionRowSpan = sectionRowSpan;
        }

        public void SetGroup(bool isUseGroup, int groupRowSpan, CashType type)
        {
            IsUseGroup = isUseGroup;
            GroupRowSpan = groupRowSpan;
            Type = type;
            TypeName = type.ToDescriptionString();
        }

        public void SetLabelOnly(BudgetCashflowCategoryModel cashflowCategory)
        {
            IsLabelOnly = true;
            CashflowCategoryId = cashflowCategory.Id;
            CashflowCategoryName = cashflowCategory.Name;
        }
    }
}
