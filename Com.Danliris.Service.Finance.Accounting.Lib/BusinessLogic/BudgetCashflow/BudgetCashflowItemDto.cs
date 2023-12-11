using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using System.Collections.Generic;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowItemDto
    {
        public BudgetCashflowItemDto()
        {

        }

        public BudgetCashflowItemDto(int cashflowTypeId, string cashflowTypeName, bool isUseSection)
        {
            CashflowTypeId = cashflowTypeId;
            CashflowTypeName = cashflowTypeName;
            IsUseSection = isUseSection;
        }

        public BudgetCashflowItemDto(BudgetCashflowCategoryModel cashflowCategory)
        {
            IsLabelOnly = true;
            CashflowCategoryId = cashflowCategory.Id;
            CashflowCategoryName = cashflowCategory.Name;
        }

        public BudgetCashflowItemDto(bool isShowSubCategoryLabel, BudgetCashflowUnitDto item, List<CurrencyDto> currencies)
        {
            var currency = currencies.FirstOrDefault(element => element.Id == item.CashflowUnit.CurrencyId);
            IsShowSubCategoryLabel = isShowSubCategoryLabel;
            SubCategoryId = item.CashflowSubCategory.Id;
            SubCategoryName = item.CashflowSubCategory.Name;
            IsReadOnly = item.CashflowSubCategory.IsReadOnly;
            Currency = currency;
            Nominal = item.CashflowUnit.Nominal;
            CurrencyNominal = item.CashflowUnit.CurrencyNominal;
            Total = item.CashflowUnit.Total;
        }

        public BudgetCashflowItemDto(bool isShowTotalLabel, string totalLabel, TotalCashType totalCashType, List<CurrencyDto> currencies)
        {
            var currency = currencies.FirstOrDefault(element => element.Id == totalCashType.CurrencyId);
            IsShowTotalLabel = isShowTotalLabel;
            TotalLabel = totalLabel;
            Currency = currency;
            Nominal = totalCashType.Nominal;
            CurrencyNominal = totalCashType.CurrencyNominal;
            Total = totalCashType.Total;
        }

        public BudgetCashflowItemDto(bool isShowDifferenceLabel, string differenceLabel, TotalCashType differenceCashType, List<CurrencyDto> currencies, bool isShowDifference)
        {
            var currency = currencies.FirstOrDefault(element => element.Id == differenceCashType.CurrencyId);
            IsShowDifferenceLabel = isShowDifferenceLabel;
            DifferenceLabel = differenceLabel;
            Currency = currency;
            Nominal = differenceCashType.Nominal;
            CurrencyNominal = differenceCashType.CurrencyNominal;
            Total = differenceCashType.Total;
            IsShowDifference = isShowDifference;
        }

        public BudgetCashflowItemDto(bool isShowSubCategoryLabel, BudgetCashflowSubCategoryModel subCategory, CurrencyDto currency)
        {
            IsShowSubCategoryLabel = isShowSubCategoryLabel;
            SubCategoryId = subCategory.Id;
            SubCategoryName = subCategory.Name;
            IsReadOnly = subCategory.IsReadOnly;
            Currency = currency;

            Items = new List<UnitItemDto>();
        }

        public BudgetCashflowItemDto(bool isShowSummaryLabel, string label, CurrencyDto currency, double nominal, double currencyNominal, double actual, bool isShowSummary)
        {
            Currency = currency;
            Nominal = nominal;
            CurrencyNominal = currencyNominal;
            IsShowSummaryLabel = isShowSummaryLabel;
            IsShowSummary = isShowSummary;
            Total = actual;
            SummaryLabel = label;
        }

        public BudgetCashflowItemDto(bool isShowSummaryBalance, string label, CurrencyDto currency, double nominal, double currencyNominal, double total, bool isSummaryBalance, string type = "summary", bool isReadOnly = false)
        {
            IsShowSummaryBalance = isShowSummaryBalance;
            SummaryBalanceLabel = label;
            IsSummaryBalance = isSummaryBalance;
            IsReadOnly = isReadOnly;
            Currency = currency;
            Nominal = nominal;
            CurrencyNominal = currencyNominal;
            Total = total;
        }

        public BudgetCashflowItemDto(bool isShowRealCashBalanceLabel, RealCashBalanceModel realCashBalance, CurrencyDto currency)
        {
            Currency = currency;
            IsShowRealCashBalanceLabel = isShowRealCashBalanceLabel;
            Currency = currency;
            Nominal = realCashBalance.Nominal;
            CurrencyNominal = realCashBalance.CurrencyNominal;
            Total = realCashBalance.Total;
            IsRealCashBalance = true;
        }

        public BudgetCashflowItemDto(bool isShowCurrencyRateLabel, CurrencyDto currency)
        {
            Currency = currency;
            IsShowCurrencyRateLabel = isShowCurrencyRateLabel;
            IsShowCurrencyRate = true;
        }

        public BudgetCashflowItemDto(bool isShowRealCashDifferenceLabel, string label, CurrencyDto currency, double nominal, double currencyNominal, double total)
        {
            Currency = currency;
            Nominal = nominal;
            CurrencyNominal = currencyNominal;
            Total = total;
            RealCashDifferenceLabel = label;
            IsShowRealCashDifferenceLabel = isShowRealCashDifferenceLabel;
            IsShowRealCashDifference = true;
        }

        public BudgetCashflowItemDto(string label, double total)
        {
            EquivalentDifferenceLabel = label;
            IsEquivalentDifference = true;
            Total = total;
        }

        public BudgetCashflowItemDto(List<UnitItemDto> items, int cashflowTypeId, string cashflowTypeName, bool isUseSection, int sectionRowSpan, bool isUseGroup, int groupRowSpan, CashType type, string typeName, bool isLabelOnly, int cashflowCategoryId, string cashflowCategoryName, bool isShowSubCategoryLabel, int subCategoryId, string subCategoryName, bool isReadOnly, CurrencyDto currency, bool isShowCurrencyRateLabel, bool isShowCurrencyRate, bool isShowRealCashBalanceLabel, double nominal, double currencyNominal, bool isShowSummaryLabel, bool isShowSummary, string equivalentDifferenceLabel, bool isEquivalentDifference, double total, bool isRealCashBalance, string realCashDifferenceLabel, bool isShowRealCashDifferenceLabel, bool isShowRealCashDifference, string summaryLabel, bool isShowDifference, bool isShowTotalLabel, string totalLabel, bool isShowDifferenceLabel, string differenceLabel, bool isShowSummaryBalance, string summaryBalanceLabel, bool isSummaryBalance)
        {
            Items = items;
            CashflowTypeId = cashflowTypeId;
            CashflowTypeName = cashflowTypeName;
            IsUseSection = isUseSection;
            SectionRowSpan = sectionRowSpan;
            IsUseGroup = isUseGroup;
            GroupRowSpan = groupRowSpan;
            Type = type;
            TypeName = typeName;
            IsLabelOnly = isLabelOnly;
            CashflowCategoryId = cashflowCategoryId;
            CashflowCategoryName = cashflowCategoryName;
            IsShowSubCategoryLabel = isShowSubCategoryLabel;
            SubCategoryId = subCategoryId;
            SubCategoryName = subCategoryName;
            IsReadOnly = isReadOnly;
            Currency = currency;
            IsShowCurrencyRateLabel = isShowCurrencyRateLabel;
            IsShowCurrencyRate = isShowCurrencyRate;
            IsShowRealCashBalanceLabel = isShowRealCashBalanceLabel;
            Nominal = nominal;
            CurrencyNominal = currencyNominal;
            IsShowSummaryLabel = isShowSummaryLabel;
            IsShowSummary = isShowSummary;
            EquivalentDifferenceLabel = equivalentDifferenceLabel;
            IsEquivalentDifference = isEquivalentDifference;
            Total = total;
            IsRealCashBalance = isRealCashBalance;
            RealCashDifferenceLabel = realCashDifferenceLabel;
            IsShowRealCashDifferenceLabel = isShowRealCashDifferenceLabel;
            IsShowRealCashDifference = isShowRealCashDifference;
            SummaryLabel = summaryLabel;
            IsShowDifference = isShowDifference;
            IsShowTotalLabel = isShowTotalLabel;
            TotalLabel = totalLabel;
            IsShowDifferenceLabel = isShowDifferenceLabel;
            DifferenceLabel = differenceLabel;
            IsShowSummaryBalance = isShowSummaryBalance;
            SummaryBalanceLabel = summaryBalanceLabel;
            IsSummaryBalance = isSummaryBalance;
        }

        public List<UnitItemDto> Items { get; private set; }

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
        public bool IsReadOnly { get; private set; }
        public CurrencyDto Currency { get; private set; }
        public bool IsShowCurrencyRateLabel { get; private set; }
        public bool IsShowCurrencyRate { get; private set; }
        public bool IsShowRealCashBalanceLabel { get; private set; }
        public double Nominal { get; private set; }
        public double CurrencyNominal { get; private set; }
        public bool IsShowSummaryLabel { get; private set; }
        public bool IsShowSummary { get; private set; }
        public string EquivalentDifferenceLabel { get; private set; }
        public bool IsEquivalentDifference { get; private set; }
        public double Total { get; private set; }
        public bool IsRealCashBalance { get; private set; }
        public string RealCashDifferenceLabel { get; private set; }
        public bool IsShowRealCashDifferenceLabel { get; private set; }
        public bool IsShowRealCashDifference { get; private set; }
        public string SummaryLabel { get; private set; }
        public bool IsShowDifference { get; private set; }
        public bool IsShowTotalLabel { get; private set; }
        public string TotalLabel { get; private set; }
        public bool IsShowDifferenceLabel { get; private set; }
        public string DifferenceLabel { get; private set; }
        public bool IsShowSummaryBalance { get; }
        public string SummaryBalanceLabel { get; }
        public bool IsSummaryBalance { get; }

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
