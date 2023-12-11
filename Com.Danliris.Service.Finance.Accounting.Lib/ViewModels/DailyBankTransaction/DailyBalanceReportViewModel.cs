using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction
{
    public class DailyBalanceReportViewModel : DailyBalanceCurrencyReportViewModel
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
    }

    public class DailyBalanceCurrencyReportViewModel
    {
        public string CurrencyCode { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
    }

}
