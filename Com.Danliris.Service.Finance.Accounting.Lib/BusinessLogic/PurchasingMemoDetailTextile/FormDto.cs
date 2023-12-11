using Com.Danliris.Service.Finance.Accounting.Lib.Enums;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile
{
    public class FormDto : IValidatableObject
    {
        public DateTimeOffset Date { get; set; }
        public DivisionDto Division { get; set; }
        public CurrencyDto Currency { get; set; }
        public bool SupplierIsImport { get; set; }
        public PurchasingMemoType Type { get; set; }
        public List<FormItemDto> Items { get; set; }
        public List<FormDetailDto> Details { get; set; }
        public string Remark { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Division == null || Division.Id <= 0)
                yield return new ValidationResult("Divisi diisi", new List<string> { "Division" });

            if (Currency == null || Currency.Id <= 0)
                yield return new ValidationResult("Mata Uang diisi", new List<string> { "Currency" });

            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "Date" });

            if (Type == PurchasingMemoType.Disposition)
            {
                if (Items == null || Items.Count.Equals(0))
                {
                    yield return new ValidationResult("List Disposisi harus diisi", new List<string> { "Item" });
                }
                else if (Items.Count > 0)
                {
                    int CountItemsError = 0;
                    string ItemsError = "[";

                    foreach (var item in Items)
                    {
                        ItemsError += "{ ";
                        if (item.Disposition == null || item.Disposition.Id <= 0)
                        {
                            CountItemsError++;
                            ItemsError += "'Disposition': 'Disposisi harus diisi', ";
                        }

                        ItemsError += "}, ";
                    }

                    ItemsError += "]";

                    if (CountItemsError > 0)
                        yield return new ValidationResult(ItemsError, new List<string> { "Items" });

                }
            }
            else
            {
                if (Details == null || Details.Count.Equals(0))
                {
                    yield return new ValidationResult("List SPB harus diisi", new List<string> { "Item" });
                }
                else if (Details.Count > 0)
                {
                    int CountItemsError = 0;
                    string ItemsError = "[";

                    foreach (var detail in Details)
                    {
                        ItemsError += "{ ";
                        if (detail.UnitPaymentOrder == null || detail.UnitPaymentOrder.Id <= 0)
                        {
                            CountItemsError++;
                            ItemsError += "'UnitPaymentOrder': 'SPB harus diisi', ";
                        }

                        ItemsError += "}, ";
                    }

                    ItemsError += "]";

                    if (CountItemsError > 0)
                        yield return new ValidationResult(ItemsError, new List<string> { "Details" });

                }
            }
        }
    }
}
