using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashFlowAccountingUnitDto
    {
        public string apiVersion { get; set; }
        public string statusCode { get; set; }
        public string message { get; set; }
        public List<BudgetCashFlowAccountingUnitItemDto> data { get; set; }
    }
}
