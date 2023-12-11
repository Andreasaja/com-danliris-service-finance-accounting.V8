using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO
{
    public class VBRealizationDocumentNonPOExpenditureItemViewModel : BaseViewModel
    {
        public DateTimeOffset? DateDetail { get; set; }
        public string Remark { get; set; }
        public string BLAWBNumber { get; set; }
        public decimal Amount { get; set; }
        public bool IsGetPPn { get; set; }
        public bool IsGetPPh { get; set; }
        public decimal PPnAmount { get; set; }
        public decimal PPhAmount { get; set; }
        public VatTaxViewModel VatTax { get; set; }
        public IncomeTaxViewModel IncomeTax { get; set; }
        public string IncomeTaxBy { get; set; }
        public decimal Total { get; set; }
    }
}
