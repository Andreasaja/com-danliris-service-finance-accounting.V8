using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashFlowAccountingUnitItemDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string COACode { get; set; }
        public int VBDocumentLayoutOrder { get; set; }
        public int AccountingUnitId { get; set; }
        public string UId { get; set; }
        public int Id { get; set; }
        public BudgetCasFlowAccountingDivisionDto Division { get; set; }
    }
}
