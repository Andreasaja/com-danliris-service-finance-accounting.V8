using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel
{
    public class PaymentDispositionNoteViewModel : BaseViewModel, IValidatableObject
    {
        public string BGCheckNumber { get; set; }

        public AccountBankViewModel AccountBank { get; set; }
        public string PaymentDispositionNo { get; set; }
        public double Amount { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        public string BankAccountCOA { get; set; }
        public string TransactionType { get; set; }
        public List<PaymentDispositionNoteItemViewModel> Items { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public double CurrencyRate { get; set; }
        public bool IsPosted { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.AccountBank == null || this.AccountBank.Id == 0)
            {
                yield return new ValidationResult("Bank harus dipilih", new List<string> { "bank" });
            }
            else
            {
                if (AccountBank.Currency.Code == "IDR")
                {
                    if (string.IsNullOrEmpty(CurrencyCode))
                    {
                        yield return new ValidationResult("Mata Uang harus dipilih", new List<string> { "Currency" });
                    }

                    if (CurrencyRate <= 0)
                    {
                        yield return new ValidationResult("Rate harus > 0", new List<string> { "CurrencyRate" });
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(TransactionType))
                yield return new ValidationResult("Jenis Transaksi harus dipilih", new List<string> { "TransactionType" });

            if (this.Supplier == null || this.Supplier.Id == 0)
            {
                yield return new ValidationResult("Supplier harus dipilih", new List<string> { "Supplier" });
            }

            if (this.PaymentDate.Equals(DateTimeOffset.MinValue) || this.PaymentDate == null)
            {
                yield return new ValidationResult("Tanggal Pembayaran harus diisi", new List<string> { "PaymentDate" });
            }
            if (this.PaymentDate.AddHours(7).Date > DateTime.Now.Date)
            {
                yield return new ValidationResult("Tanggal Pembayaran harus kurang dari atau sama dengan hari ini", new List<string> { "PaymentDate" });
            }
            if (this.Items == null || this.Items.Count == 0)
            {
                yield return new ValidationResult("Item tidak boleh kosong", new List<string> { "Items" });
            }

        }
    }
}
