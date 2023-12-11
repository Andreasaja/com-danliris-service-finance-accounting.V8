using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.DebtorCard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.DebtorCard
{
    public interface IGarmentFinanceDebtorCardReportService
    {
        List<GarmentFinanceDebtorCardReportViewModel> GetMonitoring(int month, int year, string buyer, int offset);
        Tuple<MemoryStream, string> GenerateExcel(int month, int year, string buyer, int offset);
    }
}
