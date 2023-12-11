using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Adjustment
{
    public class GarmentFinanceAdjustmentItemViewModel : BaseViewModel
    {
        public virtual int MemorialId { get; set; }
        public COAViewModel COA { get; set; }
        public string Description { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
    }
}
