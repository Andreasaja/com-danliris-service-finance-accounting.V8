using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalBankCashReceiptMonthlyRecap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalBankCashReceiptMonthlyRecap
{
    public interface IGarmentFinanceLocalBankCashReceiptMonthlyRecapService
    {
        List<GarmentFinanceLocalBankCashReceiptMonthlyRecapViewModel> GetMonitoring(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offset);
        Tuple<MemoryStream, string> GenerateExcel(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offset);
    }
}
