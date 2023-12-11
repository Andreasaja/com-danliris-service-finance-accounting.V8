using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.OthersExpenditureProofDocument
{
    public class OthersExpenditureProofDocumentReportViewModel: BaseViewModel
    {
        public string DocumentNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public string BankName { get; set; }
        public string BankCOANo { get; set; }
        public int BankCOAId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string CurrencyCode { get; set; }
        //public double Total { get; set; }
        public string Type { get; set; }
        public string CekBgNo { get; set; }
        public string Remark { get; set; }
        public int AccountBankId { get; set; }
        public bool IsPosted { get; set; }
        public decimal Total { get; set; }
    }
}
