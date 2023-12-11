using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Services.OthersExpenditureProofDocument
{
    public interface IOthersExpenditureProofDocumentService
    {
        Task<int> CreateAsync(OthersExpenditureProofDocumentCreateUpdateViewModel viewModel);
        Task<int> UpdateAsync(int id, OthersExpenditureProofDocumentCreateUpdateViewModel viewModel);
        Task<int> DeleteAsync(int id);
        Task<OthersExpenditureProofPagedListViewModel> GetPagedListAsync(int page, int size, string order, string keyword, string filter);
        Task<OthersExpenditureProofPagedListViewModel> GetLoaderAsync(string keyword, string filter);
        Task<OthersExpenditureProofDocumentViewModel> GetSingleByIdAsync(int id);
        Task<OthersExpenditureProofDocumentPDFViewModel> GetPDFByIdAsync(int id);
        Task<string> Posting(List<int> ids);
        Task<OthersExpenditureProofDocumentReportListViewModel> GetReportList(DateTimeOffset? startDate, DateTimeOffset? endDate, DateTimeOffset? dateExpenditure, string bankExpenditureNo, string division, int page, int size, string order, string keyword, string filter);
    }
}