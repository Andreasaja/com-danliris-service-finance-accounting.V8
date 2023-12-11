using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.MemorialDetailLocal
{
    public class GarmentFinanceMemorialDetailLocalViewModel : BaseViewModel, IValidatableObject
    {
        public string MemorialNo { get; set; }
        public int MemorialId { get; set; }
        public DateTimeOffset MemorialDate { get; set; }
        public decimal Amount { get; set; }
        public ChartOfAccountViewModel DebitCoa { get; set; }
        public ChartOfAccountViewModel InvoiceCoa { get; set; }

        public List<GarmentFinanceMemorialDetailLocalItemViewModel> Items { get; set; }
        public List<GarmentFinanceMemorialDetailLocalOtherItemViewModel> OtherItems { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.MemorialId == 0)
            {
                yield return new ValidationResult("No Memorial harus dipilih", new List<string> { "Memorial" });
            }

            var totalCredit = 0M;
            var totalDebit = 0M;

            totalDebit += this.Amount;

            if (this.Items == null || this.Items.Count == 0)
            {
                yield return new ValidationResult("Item tidak boleh kosong", new List<string> { "ItemsCount" });
            }
            else
            {
                int itemErrorCount = 0;
                string ItemError = "[";

                foreach (GarmentFinanceMemorialDetailLocalItemViewModel Item in Items)
                {
                    ItemError += "{ ";

                    if (Item.LocalSalesNoteId == 0)
                    {
                        itemErrorCount++;
                        ItemError += "LocalSalesNoteNo: 'No Nota harus diisi', ";
                    }
                    if (Item.Amount <= 0)
                    {
                        itemErrorCount++;
                        ItemError += "Amount: 'Jumlah harus lebih dari 0', ";
                    }

                    totalCredit += (decimal)Item.Amount;

                    ItemError += " }, ";
                }

                ItemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(ItemError, new List<string> { "Items" });
            }

            if (this.OtherItems == null || this.OtherItems.Count == 0)
            {

            } else
            {
                int itemErrorCount = 0;
                string ItemError = "[";

                foreach (GarmentFinanceMemorialDetailLocalOtherItemViewModel Item in OtherItems)
                {
                    ItemError += "{ ";

                    if (Item.Account == null)
                    {
                        itemErrorCount++;
                        ItemError += "Account: 'Account harus diisi', ";
                    }
                    if (Item.Amount <= 0)
                    {
                        itemErrorCount++;
                        ItemError += "Amount: 'Jumlah harus lebih dari 0', ";
                    }

                    if (Item.TypeAmount == "KREDIT")
                    {
                        totalCredit += Item.Amount;
                    }
                    else
                    {
                        totalDebit += Item.Amount;
                    }

                    ItemError += " }, ";
                }

                ItemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(ItemError, new List<string> { "OtherItems" });
            }

            if (totalCredit != totalDebit)
            {
                yield return new ValidationResult("Jumlah Kredit dan Debit Tidak Sama !", new List<string> { "Amount" });
            }
        }
    }
}
