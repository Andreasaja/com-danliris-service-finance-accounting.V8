using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowMasterDto
    {
        public BudgetCashflowMasterDto(BudgetCashflowTypeModel cashflowType, BudgetCashflowCategoryModel cashflowTypeWithCategory, BudgetCashflowSubCategoryModel cashflowCategoryWithSubCategory)
        {
            CashflowType = cashflowType.Name;
            CashflowTypeId = cashflowType.Id;
            CashflowTypeLayoutOrder = cashflowType.LayoutOrder;


            if (cashflowTypeWithCategory != null)
            {
                CashflowCategoryId = cashflowTypeWithCategory.Id;
                CashType = cashflowTypeWithCategory.Type.ToDescriptionString();
                CashflowCategory = cashflowTypeWithCategory.Name;
                CashflowCategoryLayoutOrder = cashflowTypeWithCategory.LayoutOrder;
            }

            if (cashflowCategoryWithSubCategory != null)
            {
                CashflowSubCategoryId = cashflowCategoryWithSubCategory.Id;
                CashflowSubCategory = cashflowCategoryWithSubCategory.Name;
                CashflowSubCategoryLayoutOrder = cashflowCategoryWithSubCategory.LayoutOrder;
            }
        }

        public BudgetCashflowMasterDto(int cashflowTypeId, int cashflowCategoryId, int cashflowSubCategoryId, string cashflowType, CashType cashType, string cashflowCategory, string cashflowSubCategory)
        {
            CashflowTypeId = cashflowTypeId;
            CashflowCategoryId = cashflowCategoryId;
            CashflowSubCategoryId = cashflowSubCategoryId;
            CashflowType = cashflowType;
            CashType = cashType.ToDescriptionString();
            CashflowType = cashflowType;
            CashflowCategory = cashflowCategory;
            CashflowSubCategory = cashflowSubCategory;
        }

        public int CashflowTypeId { get; private set; }
        public int CashflowTypeLayoutOrder { get; private set; }
        public int CashflowCategoryId { get; private set; }
        public int CashflowSubCategoryId { get; private set; }
        public string CashflowType { get; private set; }
        public string CashType { get; private set; }
        public string CashflowCategory { get; private set; }
        public int CashflowCategoryLayoutOrder { get; private set; }
        public string CashflowSubCategory { get; private set; }
        public int CashflowSubCategoryLayoutOrder { get; private set; }
    }
}