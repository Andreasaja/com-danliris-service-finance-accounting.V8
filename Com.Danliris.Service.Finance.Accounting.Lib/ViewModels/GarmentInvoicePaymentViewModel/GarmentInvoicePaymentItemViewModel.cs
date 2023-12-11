using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePaymentViewModel
{
    public class GarmentInvoicePaymentItemViewModel : BaseViewModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public decimal IDRAmount { get; set; }
    }
}
