using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt
{
    public class SalesReceiptDetailViewModel : BaseViewModel, IValidatableObject
    {
        public SalesInvoiceViewModel SalesInvoice { get; set; }
        public CurrencyViewModel Currency { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        [MaxLength(255)]
        public string VatType { get; set; }
        public double? Tempo { get; set; }
        public double? TotalPayment { get; set; }
        public double? TotalPaid { get; set; }
        public double? Paid { get; set; }
        public double? Nominal { get; set; }
        public double? Unpaid { get; set; }
        public double? OverPaid { get; set; }
        public bool? IsPaidOff { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
