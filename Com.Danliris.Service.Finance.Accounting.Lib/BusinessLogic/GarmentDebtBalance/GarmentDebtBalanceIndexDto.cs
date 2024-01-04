using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class GarmentDebtBalanceIndexDto
    {
        public List<GarmentDebtBalanceCardDto> Data { get; set; }
        public int Count { get; set; }
        public List<string>? Order { get; set; }
        public List<string>? Selected { get; set; }
    }
}
