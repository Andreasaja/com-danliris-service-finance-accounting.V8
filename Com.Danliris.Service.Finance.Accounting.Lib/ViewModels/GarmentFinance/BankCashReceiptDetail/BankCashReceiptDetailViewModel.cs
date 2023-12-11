using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceiptDetail
{
    public class BankCashReceiptDetailViewModel : BaseViewModel, IValidatableObject
    {
        public int BankCashReceiptId { get; set; }
        public string BankCashReceiptNo { get; set; }
        public DateTimeOffset BankCashReceiptDate { get; set; }
        public decimal Amount { get; set; }
        public ChartOfAccountViewModel DebitCoa { get; set; }
        public ChartOfAccountViewModel InvoiceCoa { get; set; }
        public virtual List<BankCashReceiptDetailItemViewModel> Items { get; set; }
        public virtual List<BankCashReceiptDetailOtherItemViewModel> OtherItems { get; set; }
        public double TotalAmount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.BankCashReceiptDate == null || this.BankCashReceiptDate == DateTimeOffset.MinValue)
            {
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "BankCashReceiptDate" });
            }

            if (this.BankCashReceiptNo == null || this.BankCashReceiptNo == "")
            {
                yield return new ValidationResult("Nomor Kwitansi harus diisi", new List<string> { "BankCashReceiptNo" });
            }

            if (this.Amount == 0 || this.Amount <= 0)
            {
                yield return new ValidationResult("Amount harus diisi", new List<string> { "Amount" });
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

                foreach (BankCashReceiptDetailItemViewModel Item in Items)
                {
                    ItemError += "{ ";

                    if (string.IsNullOrWhiteSpace(Item.InvoiceNo) || Item.InvoiceId == 0)
                    {
                        itemErrorCount++;
                        ItemError += "InvoiceNo: 'Invoice harus diisi', ";
                    }

                    if (Item.BuyerAgent == null || string.IsNullOrWhiteSpace(Item.BuyerAgent.Code))
                    {
                        itemErrorCount++;
                        ItemError += "BuyerCode: 'Buyer Tidak Ditemukan',";
                        ItemError += "BuyerName: 'Buyer Tidak Ditemukan',";
                    }

                    if (Item.Currency == null || string.IsNullOrWhiteSpace(Item.Currency.Code) || Item.Currency.Id == 0)
                    {
                        itemErrorCount++;
                        ItemError += "CurrencyCode: 'Kurs Tidak Ditemukan',";
                    }

                    if(Item.Amount == 0 || Item.Amount <= 0)
                    {
                        itemErrorCount++;
                        ItemError += "Amount: 'Jumlah harus diisi',";
                    }

                    totalCredit += Item.Amount;

                    ItemError += " }, ";
                }

                ItemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(ItemError, new List<string> { "Items" });
            }

            if (this.OtherItems == null || this.OtherItems.Count == 0)
            {
                yield return new ValidationResult("Detail Lain Lain tidak boleh kosong", new List<string> { "OtherItemsCount" });
            } else
            {
                int itemErrorCount = 0;
                string ItemError = "[";

                foreach (BankCashReceiptDetailOtherItemViewModel Item in OtherItems)
                {
                    ItemError += "{ ";

                    if (Item.Account == null || string.IsNullOrWhiteSpace(Item.Account.Code) || Item.Account.Id == "" || Item.Account.Id == "0")
                    {
                        itemErrorCount++;
                        ItemError += "Account: 'Account harus diisi', ";
                    }

                    if (Item.Currency == null || string.IsNullOrWhiteSpace(Item.Currency.Code) || Item.Currency.Id == 0)
                    {
                        itemErrorCount++;
                        ItemError += "OtherCurrencyCode: 'Kurs Tidak Ditemukan',";
                    }

                    if(Item.TypeAmount == null || string.IsNullOrWhiteSpace(Item.TypeAmount))
                    {
                        itemErrorCount++;
                        ItemError += "TypeAmount: 'Tipe Biaya harus diisi',";
                    }

                    if (Item.Amount == 0 || Item.Amount <= 0)
                    {
                        itemErrorCount++;
                        ItemError += "OtherAmount: 'Jumlah harus diisi',";
                    }

                    if(Item.TypeAmount == "KREDIT")
                    {
                        totalCredit += Item.Amount;
                    } else
                    {
                        totalDebit += Item.Amount;
                    }

                    ItemError += " }, ";
                }

                ItemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(ItemError, new List<string> { "OtherItems" });
            }

            if(totalCredit != totalDebit)
            {
                yield return new ValidationResult("Jumlah Kredit dan Debit Tidak Sama !", new List<string> { "Amount" });
            }
        }
    }
}
