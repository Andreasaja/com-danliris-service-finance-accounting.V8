using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount
{
    public class CreditorAccountViewModel : BaseViewModel
    {
        public DateTimeOffset? Date { get; set; }

        public string Products { get; set; }

        public string UnitReceiptNoteNo { get; set; }
        
        public string BankExpenditureNoteNo { get; set; }

        public string MemoNo { get; set; }
        public string CorrectionNo { get; set; }

        public string InvoiceNo { get; set; }

        public decimal DPP { get; set; }

        public decimal PPN { get; set; }

        public decimal Total { get; set; }

        public decimal Mutation { get; set; }
        public decimal MutationPayment { get; set; }

        public decimal FinalBalance { get; set; }
        public decimal DPPCurrency { get; set; }
        public decimal CurrencyRate { get; set; }

        public string Currency { get; set; }
        public string PaymentDuration { get; set; }
        public double BankExpenditureAmount { get; internal set; }
    }
}
