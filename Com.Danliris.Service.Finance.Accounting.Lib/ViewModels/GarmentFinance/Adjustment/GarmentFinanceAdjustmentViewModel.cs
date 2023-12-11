using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Adjustment
{
    public class GarmentFinanceAdjustmentViewModel : BaseViewModel, IValidatableObject
    {
        public string AdjustmentNo { get; set; }
        public DateTimeOffset? Date { get; set; }

        public CurrencyViewModel GarmentCurrency { get; set; }

        //public double GarmentCurrencyRate { get; set; }

        public double Amount { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }

        public List<GarmentFinanceAdjustmentItemViewModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {           
            if (this.GarmentCurrency == null || this.GarmentCurrency.Id == 0)
            {
                yield return new ValidationResult("Mata Uang harus dipilih", new List<string> { "GarmentCurrency" });
            }
            if(this.Date==DateTimeOffset.MinValue || this.Date == null)
            {
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "Date" });
            }
            else if (this.Date > DateTimeOffset.Now)
            {
                yield return new ValidationResult("Tanggal tidak boleh lebih dari hari ini", new List<string> { "Date" });
            }
            if (this.Items == null || this.Items.Count == 0)
            {
                yield return new ValidationResult("Item tidak boleh kosong", new List<string> { "ItemsCount" });
            }
            else
            {
                if(this.Items.Sum(a=>a.Credit) != this.Items.Sum(a => a.Debit))
                {
                    yield return new ValidationResult("Jumlah Debit dan Kredit tidak sama", new List<string> { "DebitCredit" });
                }

                int itemErrorCount = 0;
                string ItemError = "[";

                foreach (GarmentFinanceAdjustmentItemViewModel Item in Items)
                {
                    ItemError += "{ ";

                    if (Item.COA==null || Item.COA.Id == 0)
                    {
                        itemErrorCount++;
                        ItemError += "COA: 'No Acc harus diisi', ";
                    }

                    if(Item.Debit==0 && Item.Credit == 0)
                    {
                        itemErrorCount++;
                        ItemError += "Debit: 'Debet harus diisi', ";
                        itemErrorCount++;
                        ItemError += "Credit: 'Kredit harus diisi', ";
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
