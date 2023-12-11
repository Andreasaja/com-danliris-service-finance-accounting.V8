using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowDivision
    {
        public BudgetCashflowDivision()
        {
            Headers = new List<string>();
            Items = new List<BudgetCashflowDivisionItemDto>();
        }

        public BudgetCashflowDivision(List<string> headers, List<BudgetCashflowDivisionItemDto> items)
        {
            Headers = headers;
            Items = items;
        }

        public List<string> Headers { get; private set; }
        public List<BudgetCashflowDivisionItemDto> Items { get; private set; }
    }
}
