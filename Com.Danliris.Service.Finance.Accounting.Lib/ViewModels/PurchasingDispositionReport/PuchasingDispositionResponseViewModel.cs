using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionReport
{
    public class PurchasingDispositionResponseViewModel
    {
        public string apiVersion { get; set; }

        public List<PurchasingDispositionViewModel> data { get; set; }

        public string message { get; set; }

        public string statusCode { get; set; }

        public APIInfo info { get; set; }
    }
}
