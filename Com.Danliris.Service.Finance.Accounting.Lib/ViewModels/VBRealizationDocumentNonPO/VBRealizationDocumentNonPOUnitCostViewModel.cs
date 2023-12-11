using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO
{
    public class VBRealizationDocumentNonPOUnitCostViewModel : BaseViewModel
    {
        public UnitViewModel Unit { get; set; }
        public bool IsSelected { get; set; }
        public decimal Amount { get; set; }
    }
}
