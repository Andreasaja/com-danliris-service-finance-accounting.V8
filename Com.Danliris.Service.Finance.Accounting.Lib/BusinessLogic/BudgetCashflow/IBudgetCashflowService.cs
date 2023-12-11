using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public interface IBudgetCashflowService
    {
        int CreateBudgetCashflowType(CashflowTypeFormDto form);
        ReadResponse<BudgetCashflowTypeModel> GetBudgetCashflowTypes(string keyword, int page, int size);
        BudgetCashflowTypeModel GetBudgetCashflowTypeById(int id);
        int UpdateBudgetCashflowType(int id, CashflowTypeFormDto form);
        int DeleteBudgetCashflowType(int id);

        int CreateBudgetCashflowCategory(CashflowCategoryFormDto form);
        ReadResponse<BudgetCashflowCategoryModel> GetBudgetCashflowCategories(string keyword, int categoryTypeId, int page, int size);
        BudgetCashflowCategoryModel GetBudgetCashflowCategoryById(int id);
        int UpdateBudgetCashflowCategory(int id, CashflowCategoryFormDto form);
        int DeleteBudgetCashflowCategories(int id);

        int CreateBudgetCashflowSubCategory(CashflowSubCategoryFormDto form);
        ReadResponse<BudgetCashflowSubCategoryModel> GetBudgetCashflowSubCategories(string keyword, int subCategoryId, int page, int size);
        BudgetCashflowSubCategoryTypeDto GetBudgetCashflowSubCategoryById(int id);
        int UpdateBudgetCashflowSubCategory(int id, CashflowSubCategoryFormDto form);
        int DeleteBudgetCashflowSubCategories(int id);

        ReadResponse<BudgetCashflowMasterDto> GetBudgetCashflowMasterLayout(string keyword, int page, int size);
        int CreateBudgetCashflowUnit(CashflowUnitFormDto form);
        int UpdateBudgetCashflowUnit(CashflowUnitFormDto form);
        Task<List<BudgetCashflowItemDto>> GetBudgetCashflowUnit(int unitId, DateTimeOffset date);
        List<BudgetCashflowUnitItemDto> GetBudgetCashflowUnit(int unitId, int subCategoryId, DateTimeOffset date);
        Task<BudgetCashflowDivision> GetBudgetCashflowDivision(int divisionId, DateTimeOffset date);

        int CreateInitialCashBalance(CashBalanceFormDto form);
        int UpdateInitialCashBalance(CashBalanceFormDto form);
        List<BudgetCashflowUnitItemDto> GetInitialCashBalance(int unitId, DateTimeOffset date);

        int CreateRealCashBalance(CashBalanceFormDto form);
        int UpdateRealCashBalance(CashBalanceFormDto form);
        List<BudgetCashflowUnitItemDto> GetRealCashBalance(int unitId, DateTimeOffset date);
        Task<List<BudgetCashflowItemDto>> GetBudgetCashflowUnitAccounting(int unitAccountingId, DateTimeOffset date);
        Task<List<BudgetCashflowItemDto>> GetBudgetCashflowUnitAccountingV2(int unitAccountingId, DateTimeOffset date);
        Task<UnitDto> GetUnitAccountingById(int unitAccounting);
    }
}
