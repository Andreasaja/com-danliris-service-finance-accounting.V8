using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class ExternalPurchaseOrderViewModel : BaseViewModel
    {
        public string UId { get; set; }
        public string no { get; set; }
        public DivisionViewModel division { get; set; }
        public UnitViewModel unit { get; set; }
        public SupplierViewModel supplier { get; set; }
        public DateTimeOffset orderDate { get; set; }
        public DateTimeOffset deliveryDate { get; set; }
        public string freightCostBy { get; set; }
        public CurrencyViewModel currency { get; set; }
        public string paymentMethod { get; set; }
        public string paymentDueDays { get; set; }
        public bool useVat { get; set; }
        public IncomeTaxViewModelEPO incomeTax { get; set; }
        public string incomeTaxBy { get; set; }
        public bool useIncomeTax { get; set; }
        public bool isPosted { get; set; }
        public bool isClosed { get; set; }
        public bool isCanceled { get; set; }
        public string remark { get; set; }
        public List<ExternalPurchaseOrderItemViewModel> items { get; set; }
    }

    public class IncomeTaxViewModelEPO
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string rate { get; set; }
    }
}
