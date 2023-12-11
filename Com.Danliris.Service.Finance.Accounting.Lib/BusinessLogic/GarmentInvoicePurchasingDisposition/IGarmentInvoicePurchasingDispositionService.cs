using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentInvoicePurchasingDisposition
{
    public interface IGarmentInvoicePurchasingDispositionService
    {
        ReadResponse<GarmentInvoicePurchasingDispositionModel> Read(int page, int size, string order, List<string> select, string keyword, string filter ="{}");
        Task<int> CreateAsync(GarmentInvoicePurchasingDispositionModel model);
        Task<GarmentInvoicePurchasingDispositionModel> ReadByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(int id, GarmentInvoicePurchasingDispositionModel model);
        Task<int> Post(GarmentInvoicePurchasingDispositionPostingViewModel form);
        ReadResponse<GarmentInvoicePurchasingDispositionItemModel> ReadDetailsByEPOId(string epoId);
		Task<List<MonitoringDispositionPayment>> GetMonitoring(string invoiceNo,  string dispositionNo,DateTimeOffset startDate, DateTimeOffset endDate, int offset);
		ReadResponse<GarmentInvoicePurchasingDispositionNoVM> GetLoader(string keyword = null, string filter = "{}");
		Task<MemoryStream> DownloadReportXls(string invoiceNo, string dispositionNo, DateTimeOffset startDate, DateTimeOffset endDate);
		

	}
}
