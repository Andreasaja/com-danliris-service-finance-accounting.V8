using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public interface IDPPVATBankExpenditureNoteService
    {
        Task<int> Create(FormDto form);
        DPPVATBankExpenditureNoteDto Read(int id);
        ReadResponse<DPPVATBankExpenditureNoteIndexDto> Read(string keyword, int page = 1, int size = 25, string order = "{}");
        Task<int> Update(int id, FormDto form);
        Task<int> Delete(int id);
        List<ReportDto> ExpenditureReport(int expenditureId, int internalNoteId, int invoiceId, int supplierId, DateTimeOffset startDate, DateTimeOffset endDate);
        List<ReportDto> ExpenditureReportDetailDO(int expenditureId, int internalNoteId, int invoiceId, int supplierId, DateTimeOffset startDate, DateTimeOffset endDate);
        ReportDto ExpenditureFromInvoice(long InvoiceId);
        Task<int> Posting(List<int> ids);
        Task<string> GetDocumentNo(string type, string bankCode, string username, DateTime date);
    }
}
