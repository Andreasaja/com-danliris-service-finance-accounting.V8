using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceipt
{
    public class BankCashReceiptViewModel : BaseViewModel, IValidatableObject
    {
        public string ReceiptNo { get; set; }
        public DateTimeOffset ReceiptDate { get; set; }

        public AccountBankViewModel Bank { get; set; }

        public ChartOfAccountViewModel DebitCoa { get; set; }

        public CurrencyViewModel Currency { get; set; }

        public string NumberingCode { get; set; }
        public string IncomeType { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public bool IsUsed { get; set; }

        public BankCashReceiptTypeViewModel BankCashReceiptType { get; set; }

        public NewBuyerViewModel Buyer { get; set; }

        public virtual List<BankCashReceiptItemViewModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ReceiptDate == null || this.ReceiptDate == DateTimeOffset.MinValue)
            {
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "ReceiptDate" });
            }
            if (this.ReceiptDate > DateTimeOffset.Now)
            {
                yield return new ValidationResult("Tanggal tidak boleh lebih dari hari ini", new List<string> { "ReceiptDate" });
            }

            if (this.Bank == null || this.Bank.Id == 0)
            {
                yield return new ValidationResult("Bank harus diisi", new List<string> { "Bank" });
            }
            if (this.Currency == null || this.Currency.Id == 0)
            {
                yield return new ValidationResult("Kurs harus dipilih", new List<string> { "Currency" });
            }
            if(BankCashReceiptType==null || BankCashReceiptType.Id == 0)
            {
                yield return new ValidationResult("Tipe Pemasukan harus dipilih", new List<string> { "BankCashReceiptType" });
            }
            else if(BankCashReceiptType.Name=="PENJUALAN EKSPOR" || BankCashReceiptType.Name == "PENJUALAN LOKAL")
            {
                if(Buyer==null || Buyer.Id == 0)
                {
                    yield return new ValidationResult("Buyer harus dipilih", new List<string> { "Buyer" });
                }
            }
            if (this.Items == null || this.Items.Count == 0)
            {
                yield return new ValidationResult("Item tidak boleh kosong", new List<string> { "ItemsCount" });
            }
            else
            {
                int itemErrorCount = 0;
                string ItemError = "[";

                foreach (BankCashReceiptItemViewModel Item in Items)
                {
                    ItemError += "{ ";
                    if(Item.AccNumber == null && Item.AccSub == null)
                    {
                        itemErrorCount++;
                        ItemError += "NoAcc: 'Salah Satu harus diisi', ";
                        ItemError += "SubAcc: 'Salah Satu harus diisi', ";
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
