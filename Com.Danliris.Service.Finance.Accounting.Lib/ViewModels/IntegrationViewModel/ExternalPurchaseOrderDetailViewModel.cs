using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class ExternalPurchaseOrderDetailViewModel : BaseViewModel
    {
        public ProductViewModel product { get; set; }
        public long poItemId { get; set; }
        public long prItemId { get; set; }

        public double defaultQuantity { get; set; }
        public double dealQuantity { get; set; }

        public UomViewModel defaultUom { get; set; }
        public UomViewModel dealUom { get; set; }

        public double pricePerDealUnit { get; set; }
        public double productPrice { get; set; }
        public double priceBeforeTax { get; set; }
        public double conversion { get; set; }
        public bool includePpn { get; set; }
        public string productRemark { get; set; }
        public double? doQuantity { get; set; }
        public double dispositionQuantity { get; set; }
    }
}
