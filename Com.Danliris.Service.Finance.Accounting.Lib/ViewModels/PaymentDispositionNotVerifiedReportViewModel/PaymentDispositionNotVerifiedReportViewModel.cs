using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNotVerifiedReportViewModel
{
    public class PaymentDispositionNotVerifiedReportViewModel :  BaseViewModel
    {
        public string DispositionNo { get; set; }
        public DateTimeOffset? VerifyDate { get; set; }
        public string SupplierName { get; set; }
        public DateTimeOffset DispositionDate { get; set; }
        public string DivisionName { get; set; }
        public double PayToSupplier { get; set; }
        public string Currency { get; set; }
        public string NotVerifiedReason { get; set; }
    }
}
