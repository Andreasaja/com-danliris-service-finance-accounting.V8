using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.CreditorAccount
{
    public class CreditorAccountModel : StandardEntity, IValidatableObject
    {
        public CreditorAccountModel()
        {

        }

        public CreditorAccountModel(string supplierName, string supplierCode, bool supplierIsImport, int divisionId, string divisionCode, string divisionName, int unitId, string unitCode, string unitName, int unitPaymentCorrectionId, string unitPaymentCorrectionNo, decimal unitPaymentCorrectionDPP, decimal unitPaymentCorrectionPPN, decimal unitPaymentCorrectionMutation, DateTimeOffset unitPaymentCorrectionDate, string unitReceiptNoteNo, string products, DateTimeOffset? unitReceiptNoteDate, decimal unitReceiptNoteDPP, decimal unitReceiptNotePPN, decimal unitReceiptMutation, int bankExpenditureNoteId, string bankExpenditureNoteNo, DateTimeOffset? bankExpenditureNoteDate, decimal bankExpenditureNoteDPP, decimal bankExpenditureNotePPN, decimal bankExpenditureNoteMutation, string memoNo, DateTimeOffset? memoDate, decimal memoDPP, decimal memoPPN, decimal memoMutation, string paymentDuration, string invoiceNo, decimal finalBalance, string currencyCode, decimal dPPCurrency, decimal currencyRate, decimal vatAmount, decimal incomeTaxAmount, string externalPurchaseOrderNo)
        {
            SupplierName = supplierName;
            SupplierCode = supplierCode;
            SupplierIsImport = supplierIsImport;
            DivisionId = divisionId;
            DivisionCode = divisionCode;
            DivisionName = divisionName;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            UnitPaymentCorrectionId = unitPaymentCorrectionId;
            UnitPaymentCorrectionNo = unitPaymentCorrectionNo;
            UnitPaymentCorrectionDPP = unitPaymentCorrectionDPP;
            UnitPaymentCorrectionPPN = unitPaymentCorrectionPPN;
            UnitPaymentCorrectionMutation = unitPaymentCorrectionMutation;
            UnitPaymentCorrectionDate = unitPaymentCorrectionDate;
            UnitReceiptNoteNo = unitReceiptNoteNo;
            Products = products;
            UnitReceiptNoteDate = unitReceiptNoteDate;
            UnitReceiptNoteDPP = unitReceiptNoteDPP;
            UnitReceiptNotePPN = unitReceiptNotePPN;
            UnitReceiptMutation = unitReceiptMutation;
            BankExpenditureNoteId = bankExpenditureNoteId;
            BankExpenditureNoteNo = bankExpenditureNoteNo;
            BankExpenditureNoteDate = bankExpenditureNoteDate;
            BankExpenditureNoteDPP = bankExpenditureNoteDPP;
            BankExpenditureNotePPN = bankExpenditureNotePPN;
            BankExpenditureNoteMutation = bankExpenditureNoteMutation;
            MemoNo = memoNo;
            MemoDate = memoDate;
            MemoDPP = memoDPP;
            MemoPPN = memoPPN;
            MemoMutation = memoMutation;
            PaymentDuration = paymentDuration;
            InvoiceNo = invoiceNo;
            FinalBalance = finalBalance;
            CurrencyCode = currencyCode;
            DPPCurrency = dPPCurrency;
            CurrencyRate = currencyRate;
            VATAmount = vatAmount;
            IncomeTaxAmount = incomeTaxAmount;
            ExternalPurchaseOrderNo = externalPurchaseOrderNo;
        }
        #region Supplier
        [MaxLength(512)]
        public string SupplierName { get; set; }
        [MaxLength(128)]
        public string SupplierCode { get; set; }

        public bool SupplierIsImport { get; set; }
        #endregion

        #region Division
        public int DivisionId { get; set; }

        public string DivisionCode { get; set; }

        public string DivisionName { get; set; }
        #endregion

        #region Unit
        public int UnitId { get; set; }

        public string UnitCode { get; set; }

        public string UnitName { get; set; }
        #endregion

        #region Unit Payment Correction
        public int UnitPaymentCorrectionId { get; set; }
        [MaxLength(128)]
        public string UnitPaymentCorrectionNo { get; set; }
        public decimal UnitPaymentCorrectionDPP { get; set; }
        public decimal UnitPaymentCorrectionPPN { get; set; }
        public decimal UnitPaymentCorrectionMutation { get; set; }
        public DateTimeOffset? UnitPaymentCorrectionDate { get; set; }
        #endregion

        #region UnitReceiptNote
        [MaxLength(128)]
        public string UnitReceiptNoteNo { get; set; }

        public string Products { get; set; }

        public DateTimeOffset? UnitReceiptNoteDate { get; set; }

        public decimal UnitReceiptNoteDPP { get; set; }

        public decimal UnitReceiptNotePPN { get; set; }

        public decimal UnitReceiptMutation { get; set; }
        public decimal IncomeTaxAmount { get; set; }
        public decimal VATAmount { get; set; }
        [MaxLength(512)]
        public string IncomeTaxNo { get; set; }
        public string ExternalPurchaseOrderNo { get; set; }
        #endregion

        #region BankExpenditureNote
        public int BankExpenditureNoteId { get; set; }
        [MaxLength(128)]
        public string BankExpenditureNoteNo { get; set; }

        public DateTimeOffset? BankExpenditureNoteDate { get; set; }

        public decimal BankExpenditureNoteDPP { get; set; }

        public decimal BankExpenditureNotePPN { get; set; }

        public decimal BankExpenditureNoteMutation { get; set; }
        #endregion

        #region Memo has been changed to SPBNo or NI No
        [MaxLength(128)]
        public string MemoNo { get; set; }

        public DateTimeOffset? MemoDate { get; set; }

        public decimal MemoDPP { get; set; }

        public decimal MemoPPN { get; set; }

        public decimal MemoMutation { get; set; }
        [MaxLength(512)]
        public string PaymentDuration { get; set; }
        #endregion
        [MaxLength(128)]
        public string InvoiceNo { get; set; }

        public decimal FinalBalance { get; set; }
        [MaxLength(64)]
        public string CurrencyCode { get; set; }

        public decimal DPPCurrency { get; set; }
        public decimal CurrencyRate { get; set; }

        public string PurchasingMemoNo { get; set; }
        public int PurchasingMemoId { get; set; }
        public double PurchasingMemoAmount { get; set; }
        public DateTimeOffset? PurchasingMemoDate { get; set; }
        public bool IsStartBalance { get; set; }

        public void SetPurchasingMemo(int purchasingMemoId, string purchasingMemoNo, double purchasingMemoAmount, DateTimeOffset? purchasingMemoDate)
        {
            PurchasingMemoId = purchasingMemoId;
            PurchasingMemoNo = purchasingMemoNo;
            PurchasingMemoAmount = purchasingMemoAmount;
            PurchasingMemoDate = purchasingMemoDate;
        }

        public void RemovePurchasingMemo()
        {
            PurchasingMemoId = 0;
            PurchasingMemoNo = null;
            PurchasingMemoAmount = 0;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
