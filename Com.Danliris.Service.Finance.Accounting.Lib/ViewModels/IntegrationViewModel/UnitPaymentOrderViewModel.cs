using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class UnitPaymentOrderViewModel : BaseViewModel
    {
        public string UId { get; set; }
        public string no { get; set; }
        public DivisionViewModel division { get; set; }
        public SupplierViewModel supplier { get; set; }
        public DateTimeOffset? date { get; set; }
        public CategoryViewModel category { get; set; }
        public CurrencyViewModel currency { get; set; }
        public string paymentMethod { get; set; }
        public string invoiceNo { get; set; }
        public DateTimeOffset? invoiceDate { get; set; }
        public string pibNo { get; set; }

        public bool useIncomeTax { get; set; }
        public IncomeTaxUPOViewModel incomeTax { get; set; }
        public string incomeTaxNo { get; set; }
        public DateTimeOffset? incomeTaxDate { get; set; }
        public string incomeTaxBy { get; set; }

        public bool useVat { get; set; }
        public string vatNo { get; set; }
        public DateTimeOffset? vatDate { get; set; }

        public string remark { get; set; }
        public DateTimeOffset dueDate { get; set; }
        public bool IsCorrection { get; set; }
        public bool isPaid { get; set; }

        public int position { get; set; }

        public List<UnitPaymentOrderItemViewModel> items { get; set; }
    }
    public class IncomeTaxUPOViewModel
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string rate { get; set; }
    }
}
