using Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class ReportDto
    {
        public ReportDto(DPPVATBankExpenditureNoteDetailModel detail, DPPVATBankExpenditureNoteItemModel itemDetail, DPPVATBankExpenditureNoteModel documentItem)
        {
            ExpenditureId = documentItem.Id;
            ExpenditureNoteNo = documentItem.DocumentNo;
            ExpenditureDate = documentItem.Date;
            CategoryName = detail.CategoryName;
            PaymentMethod = detail.PaymentMethod;
            DPP = itemDetail.DPP;
            VAT = itemDetail.VATAmount;
            Amount = documentItem.Amount;
            CurrencyCode = documentItem.CurrencyCode;
            CurrencyRate = documentItem.CurrencyRate;
            BankName = documentItem.BankName;
            SupplierId = itemDetail.SupplierId;
            SupplierName = itemDetail.SupplierName;
            InternalNoteId = itemDetail.InternalNoteId;
            InternalNoteNo = itemDetail.InternalNoteNo;
            InternalNoteAmount = itemDetail.TotalAmount;
            OutstandingAmount = itemDetail.OutstandingAmount;
            InvoiceId = detail.InvoiceId;
            InvoiceNo = detail.InvoiceNo;
            InvoiceAmount = detail.Amount;
            PaidAmount = itemDetail.TotalAmount;
            Difference = InternalNoteAmount - PaidAmount;
            BillsNo = detail.BillsNo;
            PaymentBills = detail.PaymentBills;
            SupplierCode = itemDetail.SupplierCode;
            DeliveryOrdersNo = detail.DeliveryOrdersNo;
            AmountDetail = detail.Amount;
        }
        public ReportDto(DPPVATBankExpenditureNoteDetailModel detail, DPPVATBankExpenditureNoteItemModel itemDetail, DPPVATBankExpenditureNoteModel documentItem,DPPVATBankExpenditureNoteDetailDoModel detailDo)
        {
            ExpenditureId = documentItem.Id;
            ExpenditureNoteNo = documentItem.DocumentNo;
            ExpenditureDate = documentItem.Date;
            CategoryName = detail.CategoryName;
            PaymentMethod = detail.PaymentMethod;
            DPP = itemDetail.DPP;
            VAT = itemDetail.VATAmount;
            Amount = documentItem.Amount;
            CurrencyCode = documentItem.CurrencyCode;
            CurrencyRate = documentItem.CurrencyRate;
            BankName = documentItem.BankName;
            SupplierId = itemDetail.SupplierId;
            SupplierName = itemDetail.SupplierName;
            InternalNoteId = itemDetail.InternalNoteId;
            InternalNoteNo = itemDetail.InternalNoteNo;
            InternalNoteAmount = itemDetail.TotalAmount;
            OutstandingAmount = itemDetail.OutstandingAmount;
            InvoiceId = detail.InvoiceId;
            InvoiceNo = detail.InvoiceNo;
            InvoiceAmount = detail.Amount;
            PaidAmount = itemDetail.TotalAmount;
            Difference = InternalNoteAmount - PaidAmount;
            BillsNo = detailDo.BillNo;
            PaymentBills = detailDo.PaymentBill;
            SupplierCode = itemDetail.SupplierCode;
            DeliveryOrdersNo = detailDo.DONo;
            AmountDetail = detailDo.TotalAmount;
            DPPVATDetailId = detail.Id;
        }

        public ReportDto(DPPVATBankExpenditureNoteDetailModel detail, DPPVATBankExpenditureNoteItemModel itemDetail, DPPVATBankExpenditureNoteModel documentItem, List<ReportDoDetailDto> detailSj)
        {
            ExpenditureId = documentItem.Id;
            ExpenditureNoteNo = documentItem.DocumentNo;
            ExpenditureDate = documentItem.Date;
            CategoryName = detail.CategoryName;
            PaymentMethod = detail.PaymentMethod;
            DPP = itemDetail.DPP;
            VAT = itemDetail.VATAmount;
            Amount = documentItem.Amount;
            CurrencyCode = documentItem.CurrencyCode;
            CurrencyRate = documentItem.CurrencyRate;
            BankName = documentItem.BankName;
            SupplierId = itemDetail.SupplierId;
            SupplierName = itemDetail.SupplierName;
            InternalNoteId = itemDetail.InternalNoteId;
            InternalNoteNo = itemDetail.InternalNoteNo;
            InternalNoteAmount = itemDetail.TotalAmount;
            OutstandingAmount = itemDetail.OutstandingAmount;
            InvoiceId = detail.InvoiceId;
            InvoiceNo = detail.InvoiceNo;
            InvoiceAmount = detail.Amount;
            PaidAmount = itemDetail.TotalAmount;
            Difference = InternalNoteAmount - PaidAmount;
            BillsNo = detail.BillsNo;
            PaymentBills = detail.PaymentBills;
            SupplierCode = itemDetail.SupplierCode;
            DeliveryOrdersNo = detail.DeliveryOrdersNo;
            AmountDetail = detail.Amount;
            DetailDO = detailSj;
        }

        public int ExpenditureId { get; private set; }
        public string ExpenditureNoteNo { get; private set; }
        public DateTimeOffset ExpenditureDate { get; private set; }
        public string CategoryName { get; private set; }
        public string PaymentMethod { get; private set; }
        public double DPP { get; private set; }
        public double VAT { get; private set; }
        public double Amount { get; private set; }
        public string CurrencyCode { get; private set; }
        public double CurrencyRate { get; private set; }
        public string BankName { get; private set; }
        public int SupplierId { get; private set; }
        public string SupplierName { get; private set; }
        public int InternalNoteId { get; private set; }
        public string InternalNoteNo { get; private set; }
        public double InternalNoteAmount { get; private set; }
        public double OutstandingAmount { get; private set; }
        public int InvoiceId { get; private set; }
        public string InvoiceNo { get; private set; }
        public double InvoiceAmount { get; private set; }
        public double PaidAmount { get; private set; }
        public double Difference { get; private set; }
        public string BillsNo { get; private set; }
        public string PaymentBills { get; private set; }
        public string DeliveryOrdersNo { get; private set; }
        public object SupplierCode { get; private set; }
        public double AmountDetail { get; private set; }
        public int DPPVATDetailId { get; set; }
        public List<ReportDoDetailDto> DetailDO { get; private set; }
    }
}