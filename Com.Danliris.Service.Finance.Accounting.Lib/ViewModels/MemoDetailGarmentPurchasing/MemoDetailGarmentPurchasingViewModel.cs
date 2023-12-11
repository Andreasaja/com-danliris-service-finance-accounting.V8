using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing
{
    public class MemoDetailGarmentPurchasingViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public int MemoId { get; set; }
        public string Remarks { get; set; }
        public bool IsPosted { get; set; }
        public ICollection<MemoDetailGarmentPurchasingDispositionViewModel> MemoDetailGarmentPurchasingDispositions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MemoId == 0)
            {
                yield return new ValidationResult("Nomor Memo harus diisi", new List<string> { "MemoId" });
            }

            //if (MemoDate == null)
            //    yield return new ValidationResult("Tanggal harus diisi", new List<string> { "MemoDate" });

            //if (string.IsNullOrWhiteSpace(GarmentCurrenciesCode) || GarmentCurrenciesId <= 0)
            //    yield return new ValidationResult("Mata uang harus diisi", new List<string> { "GarmentCurrenciesCode" });

            //if (string.IsNullOrWhiteSpace(AccountingBookType) || AccountingBookId <= 0)
            //    yield return new ValidationResult("Jenis buku harus diisi", new List<string> { "AccountingBookType" });

            if (MemoDetailGarmentPurchasingDispositions.Count > 0)
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var memoDetail in MemoDetailGarmentPurchasingDispositions)
                {
                    ItemsError += "{ ";
                    if (memoDetail.Disposition == null || memoDetail.Disposition.DispositionId <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'DispositionId': 'No. Disposisi harus diisi', ";
                    }

                    ItemsError += "}, ";
                }

                ItemsError += "]";

                if (CountItemsError > 0)
                    yield return new ValidationResult(ItemsError, new List<string> { "MemoDetailGarmentPurchasingDispositions" });
            }
        }

    }
}
