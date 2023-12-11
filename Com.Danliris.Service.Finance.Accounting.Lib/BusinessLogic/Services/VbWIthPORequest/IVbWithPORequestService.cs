using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VbWIthPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public interface IVbWithPORequestService
    {
        ReadResponse<VbRequestWIthPOList> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        ReadResponse<VbRequestWIthPOList> ReadWithDateFilter(DateTimeOffset? dateFilter, int offSet, int page, int size, string order, List<string> select, string keyword, string filter);
        Task<int> CreateAsync(VbRequestModel model, VbWithPORequestViewModel viewmodel);
        Task<VbWithPORequestViewModel> ReadByIdAsync2(int id);
        Task<int> UpdateAsync(int id, VbWithPORequestViewModel viewmodel);
        Task<int> DeleteAsync(int id);
        Task<int> MappingData(VbWithPORequestViewModel viewmodel);
    }
}