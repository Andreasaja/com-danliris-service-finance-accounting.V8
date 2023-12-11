using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePaymentViewModel
{
    public class GarmentInvoicePaymentViewModel : BaseViewModel, IValidatableObject
    {
        public string InvoicePaymentNo { get; set; }
        public DateTimeOffset PaymentDate { get; set; }

        public NewBuyerViewModel Buyer { get; set; }

        public string BGNo { get; set; }

        public CurrencyViewModel Currency { get; set; }

        public string Remark { get; set; }

        public virtual List<GarmentInvoicePaymentItemViewModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Buyer == null || this.Buyer.Id == 0)
            {
                yield return new ValidationResult("Buyer harus dipilih", new List<string> { "Buyer" });
            }
            if (string.IsNullOrWhiteSpace(BGNo))
            {
                yield return new ValidationResult("BGNo harus dipilih", new List<string> { "BGNo" });
            }
            if (this.Currency == null || this.Currency.Id == 0)
            {
                yield return new ValidationResult("Mata Uang harus dipilih", new List<string> { "Currency" });
            }
            if (this.Items == null || this.Items.Count == 0)
            {
                yield return new ValidationResult("Item tidak boleh kosong", new List<string> { "ItemsCount" });
            }
            else
            {
                int itemErrorCount = 0;
                string ItemError = "[";

                foreach (GarmentInvoicePaymentItemViewModel Item in Items)
                {
                    ItemError += "{ ";

                    if (string.IsNullOrWhiteSpace(Item.InvoiceNo) || Item.InvoiceId==0)
                    {
                        itemErrorCount++;
                        ItemError += "InvoiceNo: 'Invoice harus diisi', ";
                    }

                    ItemError += " }, ";
                }

                ItemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(ItemError, new List<string> { "Items" });
            }
        }
    }
}
