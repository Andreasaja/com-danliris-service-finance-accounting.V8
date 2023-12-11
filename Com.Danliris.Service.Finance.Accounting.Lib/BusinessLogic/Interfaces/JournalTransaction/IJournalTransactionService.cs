using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction
{
    public interface IJournalTransactionService : IBaseService<JournalTransactionModel>
    {
        (ReadResponse<JournalTransactionReportHeaderViewModel>, decimal, decimal) GetReport(int page, int size, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet);
        MemoryStream GenerateExcel(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet);
        Task<int> ReverseJournalTransactionByReferenceNo(string referenceNo);
        Task<SubLedgerReportViewModel> GetSubLedgerReport(int? coaId, int month, int year, int timeoffset);
        Task<SubLedgerXlsFormat> GetSubLedgerReportXls(int? coaId, int? month, int? year, int timeoffset);
        Task<int> PostTransactionAsync(int id);
        Task<int> PostTransactionAsync(int id, JournalTransactionModel model);
        Task<int> CreateManyAsync(List<JournalTransactionModel> models);
        List<JournalTransactionModel> ReadUnPostedTransactionsByPeriod(int month, int year, string referenceNo, string referenceType, bool isVB);
        Task<List<GeneralLedgerWrapperReportViewModel>> GetGeneralLedgerReport(DateTimeOffset startDate, DateTimeOffset endDate, int timezoneoffset);
        Task<MemoryStream> GetGeneralLedgerReportXls(DateTimeOffset startDate, DateTimeOffset endDate, int timezoneoffset);
        ReadResponse<JournalTransactionModel> ReadByDate(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet, int page, int size, string order, List<string> select, string keyword, string filter);
        List<string> GetAllReferenceNo(string keyword, bool isVB);
        List<string> GetAllReferenceType(string keyword, bool isVB);
    }
}
