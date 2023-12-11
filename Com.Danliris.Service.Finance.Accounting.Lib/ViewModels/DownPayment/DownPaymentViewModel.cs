using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment
{
    public class DownPaymentViewModel : BaseViewModel, IValidatableObject
    {
        public string DocumentNo { get; set; }
        public DateTimeOffset? DatePayment { get; set; }
        public Bank Bank { get; set; }
        public Buyer Buyer { get; set; }
        public decimal? TotalPayment { get; set; }
        public Currency Currency { get; set; }
        public string Remark { get; set; }
        public string CategoryAcceptance { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatePayment == null)
                yield return new ValidationResult("Tanggal harus di isi!", new List<string> { "DatePayment" });

            if (Bank == null || Bank.BankName.ToString() == "")
                yield return new ValidationResult("Bank Tujuan harus di isi!", new List<string> { "Bank" });

            if (Buyer == null || Buyer.Id.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Buyer harus di isi!", new List<string> { "Buyer" });

            if (TotalPayment.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Jumlah yang dibayarkan harus di isi!", new List<string> { "TotalPayment" });

            if (Currency == null || Currency.Id.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Kurs harus di isi!", new List<string> { "Currency" });

            if (string.IsNullOrWhiteSpace(CategoryAcceptance))
                yield return new ValidationResult("Kategori Bukti Penerimaan harus di isi!", new List<string> { "CategoryAcceptance" });

            if (string.IsNullOrWhiteSpace(Remark))
                yield return new ValidationResult("Untuk Pembayaran harus di isi!", new List<string> { "Remark" });

        }
    }
}