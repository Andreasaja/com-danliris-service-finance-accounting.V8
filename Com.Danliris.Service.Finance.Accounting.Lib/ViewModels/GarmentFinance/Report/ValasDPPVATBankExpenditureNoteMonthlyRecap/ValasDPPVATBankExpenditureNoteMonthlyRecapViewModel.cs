using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.ValasDPPVATBankExpenditureNoteMonthlyRecap
{
    public class ValasDPPVATBankExpenditureNoteMonthlyRecapViewModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitIDR { get; set; }
        public decimal CreditIDR { get; set; }
    }

    public class DetailValasDPPVATBankExpenditureNoteMonthlyRecapViewModel
    {
       
        public string DocumentNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public string CurrencyCode { get; set; }
        public string SupplierName { get; set; }
        public string INNo { get; set; }
        public DateTimeOffset INDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public string ProductName { get; set; }
        public string BillNo { get; set; }
        public string PaymentBill { get; set; }

        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public double Rate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitIDR { get; set; }
        public decimal CreditIDR { get; set; }
    }
}
