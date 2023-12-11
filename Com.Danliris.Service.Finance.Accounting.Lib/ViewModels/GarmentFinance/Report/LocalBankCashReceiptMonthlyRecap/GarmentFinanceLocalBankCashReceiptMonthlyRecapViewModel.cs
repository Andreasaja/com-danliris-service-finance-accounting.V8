using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalBankCashReceiptMonthlyRecap
{
    public class GarmentFinanceLocalBankCashReceiptMonthlyRecapViewModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
