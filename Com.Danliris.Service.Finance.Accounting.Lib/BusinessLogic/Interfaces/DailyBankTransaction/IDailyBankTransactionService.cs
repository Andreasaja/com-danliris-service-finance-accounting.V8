using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.DailyBankTransaction
{
    public interface IDailyBankTransactionService : IBaseService<DailyBankTransactionModel>
    {
        ReadResponse<DailyBankTransactionModel> GetReport(int bankId, int month, int year, int clientTimeZoneOffset);
        ReadResponse<DailyBankTransactionModel> GetReportAll(string referenceNo, int accountBankId, string division, DateTimeOffset? startDate, DateTimeOffset? endDate, int page = 1, int size = 25, string order = "{}", List<string> select = null, string keyword = null, string filter = "{}");
        List<DailyBalanceReportViewModel> GetDailyBalanceReport(int bankId, DateTime startDate, DateTime endDate, string divisionName);
        List<DailyBalanceCurrencyReportViewModel> GetDailyBalanceCurrencyReport(int bankId, DateTime startDate, DateTime endDate, string divisionName);
        MemoryStream GetExcel(int bankId, int month, int year, int clientTimeZoneOffset);
        List<DailyBankTransactionModel> GeneratePdf(int bankId, int month, int year, int clientTimeZoneOffset);
        double GetBeforeBalance(int bankId, int month, int year, int clientTimeZoneOffset);
        string GetDataAccountBank(int bankId);
        Task<int> DeleteByReferenceNoAsync(string referenceNo);
        Task<int> Posting(List<int> ids);
        Task<int> CreateInOutTransactionAsync(DailyBankTransactionModel model);
        MemoryStream GenerateExcelDailyBalance(int bankId, DateTime startDate, DateTime endDate, string divisionName, int clientTimeZoneOffset);
        Task<string> GetDocumentNo(string type, string bankCode, string username);
        Task<string> GetDocumentNo(string type, string bankCode, string username,DateTime date);
        ReadResponse<DailyBankTransactionModel> GetLoader(string keyword = null, string filter = "{}");

    }
}
