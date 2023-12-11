using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PaymentDispositionNote
{
    public interface IPaymentDispositionNoteService : IBaseService<PaymentDispositionNoteModel>
    {
        ReadResponse<PaymentDispositionNoteItemModel> ReadDetailsByEPOId(string epoId);
        Task<int> Post(PaymentDispositionNotePostDto form);
        List<ReportDto> GetReport(int bankExpenditureId, int dispositionId, int supplierId, int divisionId, DateTimeOffset startDate, DateTimeOffset endDate);
        MemoryStream GetXls(List<ReportDto> data);
        ReadResponse<PurchasingDispositionExpeditionModel> GetAllByPosition(int page, int size, string order, List<string> select, string keyword, string filter);
        ResponseAmountPaidandIsPosted GetAmountPaidAndIsPosted(string DispositionNo);
        MemoryStream GeneratePdfTemplate(PaymentDispositionNoteViewModel viewModel, int clientTimeZoneOffset);
    }
}
