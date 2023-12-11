using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteModel : StandardEntity
    {
        public DPPVATBankExpenditureNoteModel()
        {

        }

        public DPPVATBankExpenditureNoteModel(string documentNo, int bankAccountId, string bankAccountNumber, string bankName, string bankAccountingCode, int currencyId, string currencyCode, double currencyRate, int supplierId, string supplierName, bool isImportSupplier, string bgCheckNo, double amount, DateTimeOffset date, string bankCurrencyCode, int bankCurrencyId, double bankCurrencyRate)
        {
            DocumentNo = documentNo;
            BankAccountId = bankAccountId;
            BankAccountNumber = bankAccountNumber;
            BankName = bankName;
            BankAccountingCode = bankAccountingCode;
            BankCurrencyCode = bankCurrencyCode;
            BankCurrencyId = bankCurrencyId;
            BankCurrencyRate = bankCurrencyRate;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            CurrencyRate = currencyRate;
            SupplierId = supplierId;
            SupplierName = supplierName;
            IsImportSupplier = isImportSupplier;
            BGCheckNo = bgCheckNo;
            Amount = amount;
            Date = date;
        }

        [MaxLength(32)]
        public string DocumentNo { get; private set; }
        public int BankAccountId { get; private set; }
        [MaxLength(64)]
        public string BankAccountNumber { get; private set; }
        [MaxLength(256)]
        public string BankName { get; private set; }
        [MaxLength(32)]
        public string BankAccountingCode { get; private set; }
        [MaxLength(32)]
        public string BankCurrencyCode { get; private set; }
        public int BankCurrencyId { get; private set; }
        public double BankCurrencyRate { get; private set; }
        public int CurrencyId { get; private set; }
        [MaxLength(32)]
        public string CurrencyCode { get; private set; }
        public double CurrencyRate { get; private set; }
        public int SupplierId { get; private set; }
        [MaxLength(1024)]
        public string SupplierName { get; private set; }
        public bool IsImportSupplier { get; private set; }
        [MaxLength(256)]
        public string BGCheckNo { get; private set; }
        public double Amount { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public bool IsPosted { get; private set; }

        public void UpdateData(double amount, int supplierId, bool isImportSupplier, string supplierName, string bgCheckNo, DateTimeOffset date, double currencyRate)
        {
            Amount = amount;
            SupplierId = supplierId;
            SupplierName = supplierName;
            IsImportSupplier = isImportSupplier;
            BGCheckNo = bgCheckNo;
            Date = date;
            CurrencyRate = currencyRate;
        }

        public void Posted()
        {
            IsPosted = true;
        }
    }
}
