using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.MemoDetailGarmentPurchasing
{
    public interface IMemoDetailGarmentPurchasingService
    {
        //MemoDetailGarmentPurchasingViewModel GetDetailById(int memoId);
        Task<MemoDetailGarmentPurchasingViewModel> ReadByIdAsync(int id);
        Task<int> CreateAsync(MemoDetailGarmentPurchasingViewModel viewModel);
        ReadResponse<ListMemoDetail> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        ReadResponse<ReportRincian> GetReport(DateTimeOffset date, int page, int size, string order, List<string> select, string keyword, string filter, int valas);
        Task<int> UpdateAsync(int id, MemoDetailGarmentPurchasingViewModel viewModel);
        Task<int> DeleteAsync(int id);
        ReadResponse<ReportPDF> GetPDF(DateTimeOffset date, int page, int size, string order, List<string> select, string keyword, string filter, int valas);
        int Posting(List<int> ids);

    }
}
