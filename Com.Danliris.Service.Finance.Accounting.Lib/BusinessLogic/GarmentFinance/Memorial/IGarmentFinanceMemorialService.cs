using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial
{
    public interface IGarmentFinanceMemorialService
    {
        ReadResponse<GarmentFinanceMemorialModel> Read(int page, int size, string order, List<string> select, string keyword, string filter = "{}");
        Task<int> CreateAsync(GarmentFinanceMemorialModel model);
        Task<GarmentFinanceMemorialModel> ReadByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(int id, GarmentFinanceMemorialModel model);
    }
}
