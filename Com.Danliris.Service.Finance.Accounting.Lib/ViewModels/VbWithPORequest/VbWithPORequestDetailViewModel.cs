using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class VbWithPORequestDetailViewModel
    {
        public string no { get; set; }
        public int? _id { get; set; }
        public Unit unit { get; set; } 
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
        public IncomeTax IncomeTax { get; set; }
        public string IncomeTaxBy { get; set; }
        public VatTaxDto vattax { get; set; }
        public ICollection<VbWithPORequestDetailItemsViewModel> Details { get; set; }
    }
}