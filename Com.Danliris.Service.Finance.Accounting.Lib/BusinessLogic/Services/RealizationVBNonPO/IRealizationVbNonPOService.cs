using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO
{
    public interface IRealizationVbNonPOService
    {
        ReadResponse<RealizationVbList> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        Task<int> CreateAsync(RealizationVbModel model, RealizationVbNonPOViewModel viewmodel);
        Task<RealizationVbNonPOViewModel> ReadByIdAsync2(int id);
        Task<int> UpdateAsync(int id, RealizationVbNonPOViewModel viewmodel);
        Task<int> DeleteAsync(int id);
        Task<int> MappingData(RealizationVbNonPOViewModel viewmodel);
    }
}