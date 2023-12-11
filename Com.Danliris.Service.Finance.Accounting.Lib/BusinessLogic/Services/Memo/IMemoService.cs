using Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Memo
{
    public interface IMemoService
    {
        ReadResponse<MemoList> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        Task<int> CreateAsync(MemoModel model);
        Task<MemoModel> ReadByIdAsync(int id);
        Task<MemoModel> ReadBySalesInvoiceAsync(string salesinvoice);
        Task<int> UpdateAsync(int id, MemoModel model);
        Task<int> DeleteAsync(int id);
    }
}
