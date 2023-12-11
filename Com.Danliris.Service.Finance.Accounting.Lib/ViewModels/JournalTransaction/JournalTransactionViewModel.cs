using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction
{
    public class JournalTransactionViewModel : BaseViewModel, IValidatableObject
    {
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string ReferenceNo { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public List<JournalTransactionItemViewModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Description == null || string.IsNullOrWhiteSpace(Description))
                yield return new ValidationResult("Deskripsi harus diisi", new List<string> { "Description" });

            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "Date" });
            else if (Date > DateTimeOffset.Now)
                yield return new ValidationResult("Tanggal harus lebih kecil dari atau sama dengan tanggal sekarang", new List<string> { "Date" });

            if (string.IsNullOrWhiteSpace(ReferenceNo))
                yield return new ValidationResult("No. Referensi harus diisi", new List<string> { "ReferenceNo" });

            if (Items == null || Items.Count.Equals(0))
            {
                yield return new ValidationResult("Entry harus diisi", new List<string> { "Item" });
            }
            else if (!Items.Count.Equals(0))
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                decimal Debit = 0;
                decimal Credit = 0;

                foreach (var item in Items)
                {
                    ItemsError += "{ ";
                    if (item.COA == null || string.IsNullOrWhiteSpace(item.COA.Code))
                    {
                        CountItemsError++;
                        ItemsError += "'COA': 'Akun harus diisi', ";
                    }

                    if (item.Debit <= 0 && item.Credit <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'Debit': 'Debit harus lebih besar dari 0', ";
                        ItemsError += "'Credit': 'Kredit harus lebih besar dari 0', ";
                    }
                    else if (item.Debit > 0 && item.Credit > 0)
                    {
                        CountItemsError++;
                        ItemsError += "'Debit': 'Isi salah satu', ";
                        ItemsError += "'Credit': 'Isi salah satu', ";
                    }
                    ItemsError += "}, ";

                    Debit += item.Debit.GetValueOrDefault();
                    Credit += item.Credit.GetValueOrDefault();
                }

                ItemsError += "]";

                if (CountItemsError > 0)
                    yield return new ValidationResult(ItemsError, new List<string> { "Items" });

                if (Debit != Credit)
                    yield return new ValidationResult("Total Debit dan Kredit harus sama", new List<string> { "Differences" });

            }
        }
    }
}
