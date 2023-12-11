using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VbNonPORequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class VbNonPORequestViewModel : BaseViewModel, IValidatableObject
    {
        public string VBNo { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? DateEstimate { get; set; }
        public Unit Unit { get; set; }
        public Division Division { get; set; }
        public CurrencyVBRequest Currency { get; set; }
        public decimal Amount { get; set; }
        public string Usage { get; set; }
        public bool Approve_Status { get; set; }
        public bool Realization_Status { get; set; }
        public bool Complete_Status { get; set; }
        public bool Spinning1 { get; set; }
        public bool Spinning2 { get; set; }
        public bool Spinning3 { get; set; }
        public bool Weaving1 { get; set; }
        public bool Weaving2 { get; set; }
        public bool Finishing { get; set; }
        public bool Printing { get; set; }
        public bool Konfeksi1A { get; set; }
        public bool Konfeksi1B { get; set; }
        public bool Konfeksi2A { get; set; }
        public bool Konfeksi2B { get; set; }
        public bool Konfeksi2C { get; set; }
        public bool Umum { get; set; }
        public bool Others { get; set; }
        public string DetailOthers { get; set; }

        public string UnitLoad { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi!", new List<string> { "Date" });

            if (DateEstimate == null)
                yield return new ValidationResult("Estimasi Tanggal Realisasi harus diisi!", new List<string> { "DateEstimate" });

            if (Unit == null || Unit.Id <= 0)
                yield return new ValidationResult("Kode VB harus diisi!", new List<string> { "VBCode" });

            if (Currency == null || Currency.Id <= 0)
                yield return new ValidationResult("Mata Uang harus diisi!", new List<string> { "Currency" });

            if (Amount <= 0)
                yield return new ValidationResult("Jumlah Uang harus diisi!", new List<string> { "Amount" });

            if (string.IsNullOrWhiteSpace(Usage))
                yield return new ValidationResult("Kegunaan harus diisi!", new List<string> { "Usage" });

            if (Spinning1 == false && Spinning2 == false && Spinning3 == false && Weaving1 == false && Weaving2 == false && Finishing == false
                && Printing == false && Konfeksi1A == false && Konfeksi1B == false && Konfeksi2A == false && Konfeksi2B == false
                && Konfeksi2C == false && Umum == false && Others == false)
                yield return new ValidationResult("Beban Unit harus dipilih salah satu!", new List<string> { "UnitLoadCheck" });
            //
            if (Others == true && string.IsNullOrWhiteSpace(DetailOthers))
                yield return new ValidationResult("Isian harus diisi!", new List<string> { "DetailOthers" });
        }
    }
}