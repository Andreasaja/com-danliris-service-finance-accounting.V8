using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition
{
    public class GarmentInvoicePurchasingDispositionItemViewModel: IValidatableObject
    {
        public int Id { get; set; }
        public string DispositionNoteNo { get; set; }
        public DateTimeOffset DispositionNoteDate { get; set; }
        public DateTimeOffset DispositionNoteDueDate { get; set; }
        public int DispositionNoteId { get; set; }
        public double CurrencyTotalPaid { get; set; }
        public double TotalPaid { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public DateTimeOffset? VerificationAcceptedDate { get; set; }
        public double CurrencyRate { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public double VATAmount { get; set; }
        public double CurrencyVATAmount { get; set; }
        public double IncomeTaxAmount { get; set; }
        public double CurrencyIncomeTaxAmount { get; set; }
        public double DPPAmount { get; set; }
        public double CurrencyDPPAmount { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string VerifiedBy { get; set; }
        public DateTimeOffset? SentDate { get; set; }
        public DateTimeOffset? AcceptedDate { get; set; }
        public string ProformaNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset VerifiedDateSend { get; set; }
        public DateTimeOffset VerifiedDateReceived { get; set; }
        public string SendToPurchasingRemark { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public int purchasingDispositionExpeditionId { get; set; }
        public double TotalPaidPayment { get; set; }
        public double TotalPaidPaymentBefore { get; set; }
        public double DiffTotalPaidPayment { get; set; }

        public double PrecisionDiffTotalPaidPayment
        {
            get
            {
                return (this.TotalPaidPaymentBefore + TotalPaidPayment)- TotalPaid;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) 
        {
            if (this.TotalPaidPaymentBefore + this.TotalPaidPayment >= this.TotalPaid)
            {
                yield return new ValidationResult("Total Bayar tidak boleh ", new List<string> { "Supplier" });
            }

        }

    }
}
