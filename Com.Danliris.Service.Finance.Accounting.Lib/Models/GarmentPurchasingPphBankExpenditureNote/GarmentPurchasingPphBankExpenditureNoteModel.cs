using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote
{
    public class GarmentPurchasingPphBankExpenditureNoteModel : StandardEntity
    {

        public GarmentPurchasingPphBankExpenditureNoteModel()
        {

        }

        public GarmentPurchasingPphBankExpenditureNoteModel(string invoiceOutNumber, DateTimeOffset invoiceOutDate, DateTimeOffset dueDateStart, DateTimeOffset dueDateEnd, int incomeTaxId, string incomeTaxName, double incomeTaxRate, string accountBankCOA, string accountBankName, string accountBankNumber, string bankAddress, string bankCode, string bankName, string bankCode1, string bankCurrencyCode, int bankCurrencyId, string bankSwiftCode, bool isPosted)
        {
            InvoiceOutNumber = invoiceOutNumber;
            InvoiceOutDate = invoiceOutDate;
            DueDateStart = dueDateStart;
            DueDateEnd = dueDateEnd;
            IncomeTaxId = incomeTaxId;
            IncomeTaxName = incomeTaxName;
            IncomeTaxRate = incomeTaxRate;
            AccountBankCOA = accountBankCOA;
            AccountBankName = accountBankName;
            AccountBankNumber = accountBankNumber;
            BankAddress = bankAddress;
            BankCode = bankCode;
            BankName = bankName;
            BankCode1 = bankCode1;
            BankCurrencyCode = bankCurrencyCode;
            BankCurrencyId = bankCurrencyId;
            BankSwiftCode = bankSwiftCode;
            IsPosted = isPosted;
        }

        /// <summary>
        /// Nomor Bukti Keluar
        /// </summary>
        public string InvoiceOutNumber { get; set; }
        public DateTimeOffset InvoiceOutDate { get; set; }
        public DateTimeOffset DueDateStart { get; set; }
        public DateTimeOffset DueDateEnd { get; set; }
        public int IncomeTaxId { get; set; }
        public string IncomeTaxName { get; set; }
        public double IncomeTaxRate { get; set; }
        public string AccountBankCOA { get; set; }
        public string AccountBankName { get; set; }
        public string AccountBankNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankCode1 { get; set; }
        public string BankCurrencyCode { get; set; }
        public int BankCurrencyId { get; set; }
        public string BankSwiftCode { get; set; }
        public bool IsPosted { get; set; }
        public virtual ICollection<GarmentPurchasingPphBankExpenditureNoteItemModel> Items { get; set; }
    }
}
