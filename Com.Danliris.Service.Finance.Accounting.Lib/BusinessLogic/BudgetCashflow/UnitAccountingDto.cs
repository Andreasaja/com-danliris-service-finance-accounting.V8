using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class UnitAccountingDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int DivisionId { get; set; }
        
        public string DivisionCode { get; set; }
        
        public string DivisionName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
