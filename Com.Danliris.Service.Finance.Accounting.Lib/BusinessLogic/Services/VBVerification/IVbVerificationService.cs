using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public interface IVbVerificationService
    {
        ReadResponse<VbVerificationList> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        ReadResponse<VbVerificationList> ReadToVerified(int page, int size, string order, List<string> select, string keyword, string filter);

        //list of verification result
        ReadResponse<VbVerificationResultList> ReadVerification(int page, int size, string order, List<string> select, string keyword, string filter);
        //Task<int> UpdateAsync(int id, VbVerificationViewModel viewmodel);

        Task<int> CreateAsync(VbVerificationViewModel viewmodel);
        Task<VbVerificationViewModel> ReadById(int id);

    }
}