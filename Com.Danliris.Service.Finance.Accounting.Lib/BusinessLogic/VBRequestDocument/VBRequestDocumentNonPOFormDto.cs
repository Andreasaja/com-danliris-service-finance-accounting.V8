using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class VBRequestDocumentNonPOFormDto : IValidatableObject
    {
        public VBRequestDocumentNonPOFormDto()
        {
            Items = new List<VBRequestDocumentNonPOItemFormDto>();
        }

        public int Id { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? RealizationEstimationDate { get; set; }
        public UnitDto SuppliantUnit { get; set; }
        public CurrencyDto Currency { get; set; }
        public decimal? Amount { get; set; }
        public string Purpose { get; set; }

        public List<VBRequestDocumentNonPOItemFormDto> Items { get; set; }
        public bool IsInklaring { get; set; }
        public string NoBL { get; set; }
        public string NoPO { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "Date" });

            if (RealizationEstimationDate == null)
                yield return new ValidationResult("Estimasi Tanggal Realisasi harus diisi", new List<string> { "RealizationEstimationDate" });

            if (SuppliantUnit == null || SuppliantUnit.Id.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Unit Pemohon harus diisi", new List<string> { "SuppliantUnit" });

            if (Currency == null || Currency.Id.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Mata Uang harus diisi", new List<string> { "Currency" });

            if(Amount.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Jumlah Uang harus diisi", new List<string> { "Amount" });

            if (IsInklaring && string.IsNullOrEmpty(NoBL))
                yield return new ValidationResult("No. BL harus diisi", new List<string> { "NoBL" });

            if(string.IsNullOrEmpty(Purpose))
                yield return new ValidationResult("Kegunaan harus diisi", new List<string> { "Purpose" });

            if (Items.Count == 0)
            {
                yield return new ValidationResult("Beban unit harus diisi", new List<string> { "Item" });
            }
            else
            {
                if(Items.Count(s => s.IsSelected) == 0)
                {
                    yield return new ValidationResult("Beban unit harus dipilih minimal 1", new List<string> { "Item" });
                }
                else
                {
                    if(!Items.Where(d => d.IsSelected).All(s => s.Unit != null && s.Unit.Id.GetValueOrDefault() != 0))
                    {
                        yield return new ValidationResult("Beban Unit harus memiliki Nama Unit", new List<string> { "Item" });
                    }
                }

            }
        }
    }
}