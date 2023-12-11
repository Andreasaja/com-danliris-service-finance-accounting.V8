using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VbNonPORequest
{
    public interface IVbNonPORequestService
    {
        ReadResponse<VbRequestList> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        ReadResponse<VbRequestList> ReadWithDateFilter(DateTimeOffset? dateFilter, int offSet, int page, int size, string order, List<string> select, string keyword, string filter);
        Task<int> CreateAsync(VbRequestModel model, VbNonPORequestViewModel viewmodel);
        Task<VbRequestModel> ReadByIdAsync(int id);
        Task<VbNonPORequestViewModel> ReadByIdAsync2(int id);
        Task<int> UpdateAsync(int id, VbNonPORequestViewModel viewmodel);
        Task<int> DeleteAsync(int id);

        //Task<int> UpdateStatusAsync(int id, VbRequestModel model);
        Task<int> UpdateStatusAsync(List<VbRequestModel> listVbRequestViewModel, string username);

        Task<int> MappingData(VbNonPORequestViewModel viewmodel);
    }
}