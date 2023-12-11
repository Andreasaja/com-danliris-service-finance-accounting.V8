using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionViewModel : BaseViewModel, IValidatableObject
    {
        public string bankExpenditureNoteNo { get; set; }
        public string cashierDivisionBy { get; set; }
        public DateTimeOffset cashierDivisionDate { get; set; }
        public CurrencyViewModel currency { get; set; }
        public DateTimeOffset paymentDueDate { get; set; }
        public string proformaNo { get; set; }
        public string notVerifiedReason { get; set; }
        public int position { get; set; }
        public string sendToCashierDivisionBy { get; set; }
        public DateTimeOffset sendToCashierDivisionDate { get; set; }
        public string sendToPurchasingDivisionBy { get; set; }
        public DateTimeOffset sendToPurchasingDivisionDate { get; set; }
        public SupplierViewModel supplier { get; set; }
        public double totalPaid { get; set; }
        public double paymentCorrection { get; set; }
        public string dispositionId { get; set; }
        public DateTimeOffset dispositionDate { get; set; }
        public string dispositionNo { get; set; }
        public string verificationDivisionBy { get; set; }
        public DateTimeOffset verificationDivisionDate { get; set; }
        public DateTimeOffset verifyDate { get; set; }
        public bool useIncomeTax { get; set; }
        public double incomeTax { get; set; }
        public IncomeTaxViewModel incomeTaxVm { get; set; }
        public bool isPaid { get; set; }
        public bool isPaidPPH { get; set; }
        public bool useVat { get; set; }
        public double vat { get; set; }
        public DateTimeOffset bankExpenditureNoteDate { get; set; }
        public DateTimeOffset bankExpenditureNotePPHDate { get; set; }
        public string bankExpenditureNotePPHNo { get; set; }
        public string paymentMethod { get; set; }
        public CategoryViewModel category { get; set; }

        public DivisionViewModel division { get; set; }

        public double dpp { get; set; }
        public double vatValue { get; set; }
        public double incomeTaxValue { get; set; }
        public double payToSupplier { get; set; }
        public double AmountPaid { get; set; }
        public bool IsPosted { get; set; }
        public List<PurchasingDispositionExpeditionItemViewModel> items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.dispositionNo == null)
            {
                yield return new ValidationResult("Disposition is required", new List<string> { "purchaseDisposition" });
            }
        }
    }
}
