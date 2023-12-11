using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalDPPVATBankExpenditureNoteMonthlyRecap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalDPPVATBankExpenditureNoteMonthlyRecap
{
    public interface ILocalDPPVATBankExpenditureNoteMonthlyRecapService
    {     
        List<LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel> GetMonitoring(DateTime? dateFrom, DateTime? dateTo, int offset);
        MemoryStream GenerateExcel(DateTime? dateFrom, DateTime? dateTo, int offset);

        MemoryStream GenerateDetailExcel(DateTime? dateFrom, DateTime? dateTo, int offset);
    }
}
