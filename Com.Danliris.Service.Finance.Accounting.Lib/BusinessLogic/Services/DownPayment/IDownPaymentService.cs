using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DownPayment
{
    public interface IDownPaymentService
    {
        ReadResponse<DownPaymentList> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        Task<int> CreateAsync(DownPaymentModel model);
        Task<DownPaymentModel> ReadByIdAsync(int id);
        Task<int> UpdateAsync(int id, DownPaymentModel model);
        Task<int> DeleteAsync(int id);
    }
}