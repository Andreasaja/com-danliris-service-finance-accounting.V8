using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteDetailModel : StandardEntity
    {
        public DPPVATBankExpenditureNoteDetailModel()
        {

        }

        public DPPVATBankExpenditureNoteDetailModel(int dppVATBankExpenditureNoteId, int dppVATBankExpenditureNoteItemId, int invoiceId, string invoiceNo, DateTimeOffset invoiceDate, string productNames, int categoryId, string categoryName, double amount, string paymentMethod, string deliveryOrdersNo, string paymentBills, string billsNo)
        {
            DPPVATBankExpenditureNoteId = dppVATBankExpenditureNoteId;
            DPPVATBankExpenditureNoteItemId = dppVATBankExpenditureNoteItemId;
            InvoiceId = invoiceId;
            InvoiceNo = invoiceNo;
            InvoiceDate = invoiceDate;
            ProductNames = productNames;
            CategoryId = categoryId;
            CategoryName = categoryName;
            Amount = amount;
            PaymentMethod = paymentMethod;
            DeliveryOrdersNo = deliveryOrdersNo;
            PaymentBills = paymentBills;
            BillsNo = billsNo;
        }

        public DPPVATBankExpenditureNoteDetailModel(int dppVATBankExpenditureNoteId, int dppVATBankExpenditureNoteItemId, int invoiceId, string invoiceNo, DateTimeOffset invoiceDate, string productNames, int categoryId, string categoryName, double amount, string paymentMethod, string deliveryOrdersNo, string paymentBills, string billsNo, string detailSjJson)
        {
            DPPVATBankExpenditureNoteId = dppVATBankExpenditureNoteId;
            DPPVATBankExpenditureNoteItemId = dppVATBankExpenditureNoteItemId;
            InvoiceId = invoiceId;
            InvoiceNo = invoiceNo;
            InvoiceDate = invoiceDate;
            ProductNames = productNames;
            CategoryId = categoryId;
            CategoryName = categoryName;
            Amount = amount;
            PaymentMethod = paymentMethod;
            DeliveryOrdersNo = deliveryOrdersNo;
            PaymentBills = paymentBills;
            BillsNo = billsNo;
            DetailSJ = detailSjJson;
        }

        public int DPPVATBankExpenditureNoteId { get; private set; }
        public int DPPVATBankExpenditureNoteItemId { get; private set; }
        public int InvoiceId { get; private set; }
        [MaxLength(4000)]
        public string? InvoiceNo { get; private set; }
        public DateTimeOffset InvoiceDate { get; private set; }
        public string? ProductNames { get; private set; }
        public int CategoryId { get; private set; }
        [MaxLength(128)]
        public string? CategoryName { get; private set; }
        public double Amount { get; private set; }
        [MaxLength(512)]
        public string? PaymentMethod { get; private set; }
        public string? DeliveryOrdersNo { get; internal set; }
        public string? PaymentBills { get; private set; }
        public string? BillsNo { get; private set; }
        public string? DetailSJ { get; private set; }
        public virtual List<DPPVATBankExpenditureNoteDetailDoModel> DPPVATBankExpenditureNoteDetailDos { get; set; }
        //public string 
    }
}
