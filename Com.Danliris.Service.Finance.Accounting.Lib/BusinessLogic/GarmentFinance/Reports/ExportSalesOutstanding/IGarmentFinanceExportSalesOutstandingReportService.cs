using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.ExportSalesOutstanding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.ExportSalesOutstanding
{
    public interface IGarmentFinanceExportSalesOutstandingReportService
    {
        List<GarmentFinanceExportSalesOutstandingReportViewModel> GetMonitoring(int month, int year, string buyer, int offset);
        Task<MemoryStream>  GenerateExcel(int month, int year, string buyer, int offset);
    }
}
