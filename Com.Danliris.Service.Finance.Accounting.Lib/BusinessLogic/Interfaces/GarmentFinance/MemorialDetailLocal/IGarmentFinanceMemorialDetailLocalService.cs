using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.GarmentFinance.MemorialDetailLocal
{
    public interface IGarmentFinanceMemorialDetailLocalService
    {
        ReadResponse<GarmentFinanceMemorialDetailLocalModel> Read(int page, int size, string order, List<string> select, string keyword, string filter = "{}");
        Task<int> CreateAsync(GarmentFinanceMemorialDetailLocalModel model);
        Task<GarmentFinanceMemorialDetailLocalModel> ReadByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(int id, GarmentFinanceMemorialDetailLocalModel model);
    }
}
