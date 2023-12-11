using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class PurchasingDispositionItemViewModel : BaseViewModel
    {
        public string EPONo { get; set; }
        public string EPOId { get; set; }
        public bool UseVat { get; set; }
        public bool UseIncomeTax { get; set; }
        public IncomeTaxViewModel IncomeTax { get; set; }

        public virtual List<PurchasingDispositionDetailViewModel> Details { get; set; }
    }
}
