using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels
{
    public class GarmentPurchasingPphBankExpenditureNoteReportViewDto
    {
        public string InvoiceOutNo { get; set; }
        public DateTimeOffset PaidDate { get; set; }
        public string Category { get; set; }
        public double PPH { get; set; }
        public string CurrencyCode { get; set; }
        public string BankName { get; set; }
        public string SupplierName { get; set; }
        public string INNO { get; set; }
        public string InvoiceNo { get; set; }
        public int Id { get; set; }
    }
}
