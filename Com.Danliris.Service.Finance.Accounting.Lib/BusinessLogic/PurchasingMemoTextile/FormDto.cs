using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public class FormDto : IValidatableObject
    {
        public AccountingBookDto AccountingBook { get; set; }
        public MemoDetailDto MemoDetail { get; set; }
        public string Remark { get; set; }
        public List<FormItemDto> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AccountingBook == null || AccountingBook.Id <= 0)
                yield return new ValidationResult("Jenis Pembukuan diisi", new List<string> { "AccountingBook" });

            if (MemoDetail == null || MemoDetail.Id <= 0)
                yield return new ValidationResult("Nomor Memo diisi", new List<string> { "MemoDetail" });

            if (Items == null || Items.Count.Equals(0))
            {
                yield return new ValidationResult("List Akun harus diisi", new List<string> { "Item" });
            }
            else if (Items.Count > 0)
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var item in Items)
                {
                    ItemsError += "{ ";
                    if (item.ChartOfAccount == null || item.ChartOfAccount.Id <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'ChartOfAccount': 'COA harus diisi', ";
                    }

                    if (item.DebitAmount <= 0 && item.CreditAmount <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'DebitAmount': 'Debit harus lebih besar dari 0', ";
                        ItemsError += "'CreditAmount': 'Kredit harus lebih besar dari 0', ";
                    }
                    else if (item.DebitAmount > 0 && item.CreditAmount > 0)
                    {
                        CountItemsError++;
                        ItemsError += "'DebitAmount': 'Isi salah satu', ";
                        ItemsError += "'CreditAmount': 'Isi salah satu', ";
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
