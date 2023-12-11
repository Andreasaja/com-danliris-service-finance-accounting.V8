using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing
{
    public class MemoGarmentPurchasingViewModel : IValidatableObject
    {
        public int? Id { get; set; }
        public string MemoNo { get; set; }
        public DateTimeOffset? MemoDate { get; set; }
        public AccountingBookViewModel AccountingBook { get; set; }
        public GarmentCurrencyViewModel Currency { get; set; }
        public string Remarks { get; set; }
        public bool? IsPosted { get; set; }
        public List<MemoGarmentPurchasingDetailViewModel> MemoGarmentPurchasingDetails { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MemoDate == null)
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "MemoDate" });

            if (string.IsNullOrWhiteSpace(Currency.Code) || Currency.Id <= 0)
                yield return new ValidationResult("Mata uang harus diisi", new List<string> { "Currency" });

            if (AccountingBook == null || string.IsNullOrWhiteSpace(AccountingBook.Type) || AccountingBook.Id <= 0)
                yield return new ValidationResult("Jenis buku harus diisi", new List<string> { "AccountingBook" });

            if (MemoGarmentPurchasingDetails == null || MemoGarmentPurchasingDetails.Count.Equals(0))
                yield return new ValidationResult("Detail pembelian harus diisi", new List<string> { "MemoDetailGarmentPurchasings" });
            
            if (MemoGarmentPurchasingDetails.Count > 0)
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var memoDetail in MemoGarmentPurchasingDetails)
                {
                    ItemsError += "{ ";
                    if (string.IsNullOrWhiteSpace(memoDetail.COA.Name) || string.IsNullOrWhiteSpace(memoDetail.COA.No) || memoDetail.COA.Id <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'COA': 'Nomor COA harus diisi', ";
                    }

                    if (memoDetail.DebitNominal <= 0 && memoDetail.CreditNominal <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'DebitNominal': 'Debit harus lebih besar dari 0', ";
                        ItemsError += "'CreditNominal': 'Kredit harus lebih besar dari 0', ";
                    }
                    else if (memoDetail.DebitNominal > 0 && memoDetail.CreditNominal > 0)
                    {
                        CountItemsError++;
                        ItemsError += "'DebitNominal': 'Isi salah satu', ";
                        ItemsError += "'CreditNominal': 'Isi salah satu', ";
                    }
                    ItemsError += "}, ";
                }

                ItemsError += "]";

                if (CountItemsError > 0)
                    yield return new ValidationResult(ItemsError, new List<string> { "MemoGarmentPurchasingDetails" });

                var totalCredit = MemoGarmentPurchasingDetails.Sum(x => x.CreditNominal);
                var totalDebit = MemoGarmentPurchasingDetails.Sum(x => x.DebitNominal);
                if (totalCredit != totalDebit)
                    yield return new ValidationResult(ItemsError, new List<string> { "TotalDebit", "TotalCredit" });
            }
        }
    }
}
