using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels
{
    public class GarmentPurchasingPphBankExpenditureNoteFilterReportDto
    {
        public string InvoiceOutNo { get; set; }
        public string INNo { get; set; }
        public string InvoiceNo { get; set; }
        public string SupplierName { get; set; }
        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
    }
}
