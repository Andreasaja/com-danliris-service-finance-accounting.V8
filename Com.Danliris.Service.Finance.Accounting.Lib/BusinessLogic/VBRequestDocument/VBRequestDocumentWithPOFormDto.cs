using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class VBRequestDocumentWithPOFormDto : IValidatableObject
    {
        public int? Id { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? RealizationEstimationDate { get; set; }
        public UnitDto SuppliantUnit { get; set; }
        public CurrencyDto Currency { get; set; }
        public decimal? Amount { get; set; }
        public string Purpose { get; set; }

        public List<VBRequestDocumentWithPOItemFormDto> Items { get; set; }
        public bool IsInklaring { get; set; }
        public string TypePurchasing { get; set; }

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

            if (Amount.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Nominal harus diisi", new List<string> { "Amount" });

            if (Items == null || Items.Count.Equals(0))
            {
                yield return new ValidationResult("Beban unit harus diisi", new List<string> { "Item" });
            }
            else if (Items.Count > 0)
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var item in Items)
                {
                    ItemsError += "{ ";

                    if (item.PurchaseOrderExternal == null || item.PurchaseOrderExternal.Id.GetValueOrDefault() <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'PurchaseOrderExternal': 'PO External harus diisi', ";
                    }

                    ItemsError += "}, ";
                }

                ItemsError += "]";

                if (CountItemsError > 0)
                    yield return new ValidationResult(ItemsError, new List<string> { "Items" });
            }
        }
    }
}