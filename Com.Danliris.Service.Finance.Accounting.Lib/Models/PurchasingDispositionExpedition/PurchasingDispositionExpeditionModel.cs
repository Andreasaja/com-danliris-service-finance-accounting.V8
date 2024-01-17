using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionModel : StandardEntity, IValidatableObject
    {
        public string? BankExpenditureNoteNo { get; set; }
        public string? CashierDivisionBy { get; set; }
        public DateTimeOffset? CashierDivisionDate { get; set; }
        [MaxLength(50)]
        public string? CurrencyId { get; set; }
        [MaxLength(255)]
        public string? CurrencyCode { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public string? ProformaNo { get; set; }
        public string? NotVerifiedReason { get; set; }
        public ExpeditionPosition Position { get; set; }
        public string? SendToCashierDivisionBy { get; set; }
        public DateTimeOffset? SendToCashierDivisionDate { get; set; }
        public string? SendToPurchasingDivisionBy { get; set; }
        public DateTimeOffset? SendToPurchasingDivisionDate { get; set; }
        [MaxLength(255)]
        public string? SupplierId { get; set; }
        [MaxLength(255)]
        public string? SupplierCode { get; set; }
        [MaxLength(255)]
        public string? SupplierName { get; set; }
        public double TotalPaid { get; set; }
        public string? DispositionId { get; set; }
        public DateTimeOffset DispositionDate { get; set; }
        public string? DispositionNo { get; set; }
        public string? VerificationDivisionBy { get; set; }
        public DateTimeOffset? VerificationDivisionDate { get; set; }
        public DateTimeOffset? VerifyDate { get; set; }
        public bool UseIncomeTax { get; set; }
        //public double IncomeTax { get; set; }
        public string? IncomeTaxId { get; set; }
        public string? IncomeTaxName { get; set; }
        public double IncomeTaxRate { get; set; }
        public bool IsPaid { get; set; }
        public bool IsPaidPPH { get; set; }
        public bool UseVat { get; set; }
        //public double Vat { get; set; }
        public DateTimeOffset? BankExpenditureNoteDate { get; set; }
        public DateTimeOffset? BankExpenditureNotePPHDate { get; set; }
        public string? BankExpenditureNotePPHNo { get; set; }
        public string? PaymentMethod { get; set; }
        [MaxLength(255)]
        public string? CategoryId { get; set; }
        [MaxLength(255)]
        public string? CategoryCode { get; set; }
        [MaxLength(255)]
        public string? CategoryName { get; set; }
        [MaxLength(255)]
        public string? DivisionId { get; set; }
        [MaxLength(255)]
        public string? DivisionCode { get; set; }
        [MaxLength(255)]
        public string? DivisionName { get; set; }

        public double DPP { get; set; }
        public double VatValue { get; set; }
        public double IncomeTaxValue { get; set; }
        public double PayToSupplier { get; set; }
        public double AmountPaid { get; set; }
        public double SupplierPayment { get; set; }
        public double PaymentCorrection { get; set; }


        public virtual ICollection<PurchasingDispositionExpeditionItemModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
