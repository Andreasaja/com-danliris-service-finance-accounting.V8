using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalSalesOutstanding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalSalesOutstanding
{
    public interface IGarmentFinanceLocalSalesOutstandingReportService
    {
        List<GarmentFinanceLocalSalesOutstandingReportViewModel> GetMonitoring(int month, int year, string buyer, int offset);
        Task<MemoryStream> GenerateExcel(int month, int year, string buyer, int offset);
    }
}
