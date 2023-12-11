using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo
{
    public class MemoViewModel : BaseViewModel, IValidatableObject
    {
        public string DocumentNo { get; set; }
        public SalesInvoice SalesInvoice { get; set; }
        public string MemoType { get; set; }
        public DateTimeOffset? Date { get; set; }
        
        public Unit Unit { get; set; }
        public string Remark { get; set; }
        public List<MemoItemViewModel> Items { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SalesInvoice == null || SalesInvoice.Id.GetValueOrDefault() <= 0)
                yield return new ValidationResult("No Faktur harus diisi", new List<string> { "SalesInvoice" });

            if (string.IsNullOrWhiteSpace(MemoType))
                yield return new ValidationResult("Jenis Memo harus diisi", new List<string> { "MemoType" });

            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "Date" });

            //if (Buyer == null || Buyer.Id.GetValueOrDefault() <= 0)
            //    yield return new ValidationResult("Buyer harus diisi", new List<string> { "Buyer" });

            if (Unit == null || Unit.Id.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Unit harus diisi", new List<string> { "Unit" });

            if (Items == null || Items.Count.Equals(0))
            {
                yield return new ValidationResult("Daftar harus diisi", new List<string> { "Item" });
            }
            else if (Items.Count > 0)
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var item in Items)
                {
                    ItemsError += "{ ";
                    if (item.Currency == null || item.Currency.Id.GetValueOrDefault() <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'Currency': 'Mata Uang harus diisi', ";
                    }

                    if (item.PaymentAmount.GetValueOrDefault() <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'PaymentAmount': 'Jumlah Pembayaran harus lebih besar dari 0', ";
                    }

                    if (item.Interest.GetValueOrDefault() < 0)
                    {
                        CountItemsError++;
                        ItemsError += "'Interest': 'Bunga harus lebih besar atau sama dengan 0', ";
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
