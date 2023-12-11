using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public interface IRealizationVbWithPOService
    {
        ReadResponse<RealizationVbList> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        Task<int> CreateAsync(RealizationVbModel model, RealizationVbWithPOViewModel viewmodel);
        Task<RealizationVbWithPOViewModel> ReadByIdAsync2(int id);
        Task<int> UpdateAsync(int id, RealizationVbWithPOViewModel viewmodel);
        Task<int> DeleteAsync(int id);
        Task<int> MappingData(RealizationVbWithPOViewModel viewmodel);
    }
}