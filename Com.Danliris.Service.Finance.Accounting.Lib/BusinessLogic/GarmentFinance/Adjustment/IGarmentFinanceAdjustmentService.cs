using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Adjustment;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Adjustment
{
    public interface IGarmentFinanceAdjustmentService
    {
        ReadResponse<GarmentFinanceAdjustmentModel> Read(int page, int size, string order, List<string> select, string keyword, string filter = "{}");
        Task<int> CreateAsync(GarmentFinanceAdjustmentModel model);
        Task<GarmentFinanceAdjustmentModel> ReadByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(int id, GarmentFinanceAdjustmentModel model);
    }
}
