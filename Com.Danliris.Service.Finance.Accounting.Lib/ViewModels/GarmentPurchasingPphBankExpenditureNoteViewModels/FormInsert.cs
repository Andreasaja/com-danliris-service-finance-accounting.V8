using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels
{
    public class FormInsert
    {
        public Bank Bank { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        public IncomeTax IncomeTax { get; set; }
        public ICollection<FormAdd> PPHBankExpenditureNoteItems { get; set; }
        public string PphBankInvoiceNo { get; set; }
        public int Id { get; set; }
        public bool IsPosted { get; set; }
    }

    public class Bank
    {
        public int? Id { get; set; }
        public string SwiftCode { get; set; }
        public string AccountCOA { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Code { get; set; }
        public Currency Currency { get; set; }
    }
    public class Currency
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public int? Id { get; set; }
        public decimal? Rate { get; set; }
    }
    public class IncomeTax
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal? Rate { get; set; }
    }
    public class UnitPaymentOrders
    {
        public string BankAccountName { get; set; }
        public bool Check { get; set; }
        public DateTimeOffset? CreatedUtc { get; set; }
        public DateTimeOffset? LastModifiedUtc { get; set; }
        public string No { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }
        public decimal TotalDpp { get; set; }
        public decimal TotalIncomeTax { get; set; }
        public DateTimeOffset? UPODate { get; set; }
        public string UnitPaymentOrderList { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public int Id { get; set; }
        public string IncomeTax { get; set; }
        public string IncomeTaxName { get; set; }
        public decimal IncomeTaxRate { get; set; }
        public bool IsPosted { get; set; }
        public ICollection<InternNote> Items { get; set; }
    }
    
    public class InternNote
    {
        public string NumberOfNI { get; set; }
        public int PPHBankExpenditureNoteId { get; set; }
    }
}
