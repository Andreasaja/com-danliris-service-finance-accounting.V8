using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentDebtBalance
{
    public class GarmentDebtBalanceFilterViewModel
    {
        public int supplierId { get; set; }
        public string? supplierName { get; set; }
        public bool import { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }
}
