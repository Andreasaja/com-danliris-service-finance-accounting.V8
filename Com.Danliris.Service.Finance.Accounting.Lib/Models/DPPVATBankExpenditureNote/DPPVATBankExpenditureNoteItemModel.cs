using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteItemModel : StandardEntity
    {
        public DPPVATBankExpenditureNoteItemModel()
        {

        }

        public DPPVATBankExpenditureNoteItemModel(int dppVATBankExpenditureNoteId, int internalNoteId, string internalNoteNo, DateTimeOffset internalNoteDate, DateTimeOffset dueDate, int supplierId, string supplierName, bool isImportSupplier, double vatAmount, double incomeTaxAmount, double dpp, double totalAmount, int currencyId, string currencyCode, double outstandingAmount, string supplierCode)
        {
            DPPVATBankExpenditureNoteId = dppVATBankExpenditureNoteId;
            InternalNoteId = internalNoteId;
            InternalNoteNo = internalNoteNo;
            InternalNoteDate = internalNoteDate;
            DueDate = dueDate;
            SupplierId = supplierId;
            SupplierName = supplierName;
            SupplierCode = supplierCode;
            IsImportSupplier = isImportSupplier;
            VATAmount = vatAmount;
            IncomeTaxAmount = incomeTaxAmount;
            DPP = dpp;
            TotalAmount = totalAmount;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            OutstandingAmount = outstandingAmount;
        }

        public int DPPVATBankExpenditureNoteId { get; private set; }
        public int InternalNoteId { get; private set; }
        [MaxLength(32)]
        public string? InternalNoteNo { get; private set; }
        public DateTimeOffset InternalNoteDate { get; private set; }
        public DateTimeOffset DueDate { get; private set; }
        public int SupplierId { get; private set; }
        [MaxLength(1024)]
        public string? SupplierName { get; private set; }
        public bool IsImportSupplier { get; private set; }
        public double VATAmount { get; private set; }
        public double IncomeTaxAmount { get; private set; }
        public double DPP { get; private set; }
        public double TotalAmount { get; private set; }
        public int CurrencyId { get; private set; }
        [MaxLength(32)]
        public string? CurrencyCode { get; private set; }
        public double OutstandingAmount { get; private set; }
        public string? SupplierCode { get; private set; }
    }
}
