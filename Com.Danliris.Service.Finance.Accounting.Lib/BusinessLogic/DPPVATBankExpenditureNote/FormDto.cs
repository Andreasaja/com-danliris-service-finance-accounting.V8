using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class FormDto : IValidatableObject
    {
        public AccountBankDto Bank { get; set; }
        public CurrencyDto Currency { get; set; }
        public SupplierDto Supplier { get; set; }
        public string BGCheckNo { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset? Date { get; set; }
        public List<FormItemDto> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Bank == null || Bank.Id <= 0)
                yield return new ValidationResult("Bank harus diisi", new List<string> { "Bank" });

            if (Currency == null || Currency.Id <= 0)
                yield return new ValidationResult("Mata Uang harus diisi", new List<string> { "Currency" });

            if (Supplier == null || Supplier.Id <= 0)
                yield return new ValidationResult("Supplier harus diisi", new List<string> { "Supplier" });

            if (string.IsNullOrWhiteSpace(BGCheckNo))
                yield return new ValidationResult("No. BG/Check harus diisi", new List<string> { "BGCheckNo" });

            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "Date" });

            if (Items == null || Items.Count.Equals(0) || !Items.Any(item => item.Select))
            {
                yield return new ValidationResult("Nota Intern harus dipilih", new List<string> { "Item" });
            }
            else if (Items.Count > 0)
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var item in Items)
                {
                    ItemsError += "{ ";

                    if (item.Select)
                    {
                        if (item.InternalNote == null || item.InternalNote.Items == null || item.InternalNote.Items.Count <= 0 || !item.InternalNote.Items.Any(itemItem => itemItem.SelectInvoice))
                        {
                            CountItemsError++;
                            ItemsError += "'Invoice': 'Invoice harus dipilih', ";
                        }
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