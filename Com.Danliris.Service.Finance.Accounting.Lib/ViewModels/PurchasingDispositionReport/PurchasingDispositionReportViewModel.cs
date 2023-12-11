using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionReport
{
    public class PurchasingDispositionReportViewModel
    {
        public string DispositionNo { get; set; }

        public DateTimeOffset? CreatedUtc { get; set; }

        public DateTimeOffset? PaymentDueDate { get; set; }

        public string InvoiceNo { get; set; }

        public string SupplierName { get; set; }

        public int Position { get; set; }

        public DateTimeOffset? SentToVerificationDivisionDate { get; set; }

        public DateTimeOffset? VerificationDivisionDate { get; set; }

        public DateTimeOffset? VerifyDate { get; set; }

        public DateTimeOffset? SendDate { get; set; }

        public DateTimeOffset? CashierDivisionDate { get; set; }

        public DateTimeOffset? BankExpenditureNoteDate { get; set; }

        public string BankExpenditureNoteNo { get; set; }

        public string BankExpenditureNotePPHNo { get; set; }

        public DateTimeOffset? BankExpenditureNotePPHDate { get; set; }

        public string Staff { get; set; }
        public double PayToSupplier { get; set; }
        public string Currency { get; set; }
        public decimal DPP { get; set; }
        public decimal VAT { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal Total { get; set; }
        public int DueDateDays { get; set; }
        public string Category { get; set; }
        public string Division { get; set; }
        public string Unit { get; set; }
        public string VerifiedBy { get; set; }
        public string ExternalPurchaseOrderNo { get; set; }
        public string UnitPaymentOrderDate { get; set; }
        public string UnitPaymentOrderNo { get; set; }
        public double CurrencyRate { get; set; }
        public string DONo { get; set; }
        public string UrnNo { get; set; }
        public double DifferenceNominal { get; set; }
        public double PaymentCorrection { get; internal set; }
        public double SupplierPayment { get; internal set; }
    }
}
