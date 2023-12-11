using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class ExternalPurchaseOrderItemViewModel : BaseViewModel
    {
        public string poNo { get; set; }
        public string prNo { get; set; }
        public long poId { get; set; }
        public long prId { get; set; }

        public UnitViewModel unit { get; set; }
        public List<ExternalPurchaseOrderDetailViewModel> details { get; set; }
    }
}
