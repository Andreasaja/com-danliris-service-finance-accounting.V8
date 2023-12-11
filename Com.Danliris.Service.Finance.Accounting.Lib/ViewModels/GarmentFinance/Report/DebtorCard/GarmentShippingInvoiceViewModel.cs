using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.DebtorCard
{
    public class GarmentShippingInvoiceViewModel
    {
            public string invoiceId { get; set; }
            public string buyerAgentCode { get; set; }
            public string buyerAgentName { get; set; }
            public double amount { get; set; }
            public double balanceAmount { get; set; }
            public DateTimeOffset truckingDate { get; set; }
            public DateTimeOffset date { get; set; }
            public string invoiceNo { get; set; }
    
    }
}
