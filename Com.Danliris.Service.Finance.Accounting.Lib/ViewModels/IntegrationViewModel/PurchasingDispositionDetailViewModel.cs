using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class PurchasingDispositionDetailViewModel : BaseViewModel
    {
        public string PRId { get; set; }
        public string PRNo { get; set; }
        public CategoryViewModel Category { get; set; }
        public ProductViewModel Product { get; set; }
        public double DealQuantity { get; set; }
        public UomViewModel DealUom { get; set; }
        public double PaidQuantity { get; set; }
        public double PricePerDealUnit { get; set; }
        public double PriceTotal { get; set; }
        public double PaidPrice { get; set; }
        public UnitViewModel Unit { get; set; }
    }
}
