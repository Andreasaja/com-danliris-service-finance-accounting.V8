using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel
{
    public class AccountBankViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string BankCode { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string AccountCOA { get; set; }

        public CurrencyViewModel Currency { get; set; }
    }
}
