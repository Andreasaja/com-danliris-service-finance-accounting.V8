using Com.Danliris.Service.Finance.Accounting.Lib.Models.CreditorAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditBalance
{
    public class CreditBalanceAccountViewModel
    {
        //public decimal StartBalance { get; set; }
        public string SupplierCode { get; set; }
        public string DivisionCode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTimeOffset? UnitReceiptNoteDate { get; set; }
        public string Products { get; set; }
        public decimal UnitReceiptMutation { get; set; }
        public decimal BankExpenditureNoteMutation { get; set; }
        public decimal FinalBalance { get; set; }
        public double PaidAmount { get; set; }
        public int DivisionId { get; set; }
        public decimal IncomeTaxAmount { get; set; }
        public string SupplierName { get; set; }
        public decimal CurrencyRate { get; set; }
        public string DivisionName { get; set; }
        public decimal VATAmount { get; set; }
        public string UnitPaymentOrderNo { get; set; }
        public string ExternalPurchaseOrderNo { get; set; }
        public string UnitReceiptNoteNo { get; set; }

        public CreditBalanceAccountViewModel(CreditorAccountModel creditorAccountModel)
        {
            SupplierCode = creditorAccountModel.SupplierCode;
            DivisionCode = creditorAccountModel.DivisionCode;
            CurrencyCode = creditorAccountModel.CurrencyCode;
            UnitReceiptNoteDate = creditorAccountModel.UnitReceiptNoteDate;
            Products = creditorAccountModel.Products;
            UnitReceiptMutation = creditorAccountModel.UnitReceiptMutation;
            BankExpenditureNoteMutation = creditorAccountModel.BankExpenditureNoteMutation;
            SupplierName = creditorAccountModel.SupplierName;
            CurrencyRate = creditorAccountModel.CurrencyRate;
            DivisionName = creditorAccountModel.DivisionName;
            FinalBalance = creditorAccountModel.FinalBalance;
            PaidAmount = creditorAccountModel.PurchasingMemoAmount;
            DivisionId = creditorAccountModel.DivisionId;
            IncomeTaxAmount = creditorAccountModel.IncomeTaxAmount;
            VATAmount = creditorAccountModel.VATAmount;
            UnitPaymentOrderNo = creditorAccountModel.MemoNo;
            ExternalPurchaseOrderNo = creditorAccountModel.ExternalPurchaseOrderNo;
            UnitReceiptNoteNo = creditorAccountModel.UnitReceiptNoteNo;
        }
    }
}
