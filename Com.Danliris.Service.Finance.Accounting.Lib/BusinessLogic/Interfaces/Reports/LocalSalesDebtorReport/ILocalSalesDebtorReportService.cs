using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Reports.LocalSalesDebtorReport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.Reports.LocalSalesDebtorReport
{
    public interface ILocalSalesDebtorReportService
    {
        Task<List<LocalSalesDebtorReportViewModel>> GetMonitoring(int month, int year, int offset);
        Task<MemoryStream> GenerateExcel(int month, int year);
        Task<MemoryStream> GenerateExcelSummary(int month, int year);
    }
}
