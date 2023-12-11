using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.ValasDPPVATBankExpenditureNoteMonthlyRecap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.ValasDPPVATBankExpenditureNoteMonthlyRecap
{
    public interface IValasDPPVATBankExpenditureNoteMonthlyRecapService
    {     
        List<ValasDPPVATBankExpenditureNoteMonthlyRecapViewModel> GetMonitoring(DateTime? dateFrom, DateTime? dateTo, int offset);
        MemoryStream GenerateExcel(DateTime? dateFrom, DateTime? dateTo, int offset);

        MemoryStream GenerateDetailExcel(DateTime? dateFrom, DateTime? dateTo, int offset);
    }
}
