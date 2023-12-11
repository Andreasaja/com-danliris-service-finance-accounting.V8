using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class GarmentDebtBalanceSummaryAndTotalCurrencyDto
    {
        public List<GarmentDebtBalanceSummaryDto> Data { get; set; }
        public List<GarmentDebtBalanceSummaryTotalByCurrencyDto> GroupTotalCurrency { get; set; }
    }
}
