using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionAcceptance;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionReport;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionVerification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PurchasingDispositionExpedition
{
	public interface IPurchasingDispositionExpeditionService : IBaseService<PurchasingDispositionExpeditionModel>
	{
		Task<int> PurchasingDispositionAcceptance(PurchasingDispositionAcceptanceViewModel data);
		Task<int> DeletePurchasingDispositionAcceptance(int id);
		Task<int> PurchasingDispositionVerification(PurchasingDispositionVerificationViewModel data);
		Task<ReadResponse<PurchasingDispositionReportViewModel>> GetReportAsync(int page, int size, string order, string filter, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, DateTimeOffset? dateFromPayment, DateTimeOffset? dateToPayment, string bankExpenditureNoteNo, string SPBStatus, string PaymentStatus, int offSet);
		Task<MemoryStream> GenerateExcelAsync(int page, int size, string order, string filter, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, DateTimeOffset? dateFromPayment, DateTimeOffset? dateToPayment, string bankExpenditureNoteNo, string SPBStatus, string PaymentStatus, int offSet);
		ReadResponse<PurchasingDispositionExpeditionModel> ReadBankExpenditureNoteNo(int page, int size, string order, List<string> select, string keyword, string filter);
	}
}
