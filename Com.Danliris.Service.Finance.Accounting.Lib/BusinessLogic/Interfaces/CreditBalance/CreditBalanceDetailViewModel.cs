using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditBalance;
using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CreditBalance
{
    public class CreditBalanceDetailViewModel : CreditBalanceViewModel
    {
        public string ExternalPurchaseOrderNo { get; set; }
        public string IncomeTaxNo { get; set; }
        public string InvoiceNo { get; set; }
        public decimal DPPAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal IncomeTaxAmount { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string UnitPaymentOrderNo { get; internal set; }
        public string UnitReceiptNoteNo { get; internal set; }
        public decimal Total { get { return Purchase - Payment; } }
        public string BankExpenditureNoteNo { get; set; }
    }
}