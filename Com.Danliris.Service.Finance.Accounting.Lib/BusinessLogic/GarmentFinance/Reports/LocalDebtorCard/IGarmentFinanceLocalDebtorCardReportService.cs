using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalDebtorCard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalDebtorCard
{
    public interface IGarmentFinanceLocalDebtorCardReportService
    {
        List<GarmentFinanceLocalDebtorCardReportViewModel> GetMonitoring(int month, int year, string buyer, int offset);
        Tuple<MemoryStream, string> GenerateExcel(int month, int year, string buyer, int offset);
    }
}
